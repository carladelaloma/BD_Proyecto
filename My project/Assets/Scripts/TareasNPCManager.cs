using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using Unity.VisualScripting;
using System.Collections;
using System;
using System.Security.Cryptography;
using NUnit.Framework;

public class TareasNPCManager : MonoBehaviour
{
    public GameObject[] NPCs;

    private Dictionary<int, Vector3> salaCoordenadas = new Dictionary<int, Vector3>()
    {
        
        {1, new Vector3(-5,-3,0)},
        {2, new Vector3(5, -3, 0)},
        {3, new Vector3(-4, 3, 0)},
        {4, new Vector3(4, 3, 0)},
        {5, new Vector3(2, 0, 0)},
    };

    private string dbPath;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dbPath = "URI=file:" + Application.dataPath + "/Plugins/among_db.sqlite";
        foreach (GameObject npc in NPCs)
        {
            StartCoroutine(AsignarRolYTarea(npc));
        }
    }

    IEnumerator AsignarRolYTarea(GameObject npc)
    {
        int seed = npc.GetInstanceID() + DateTime.Now.Millisecond;
        System.Random rng = new System.Random(seed);

        yield return new WaitForSeconds((float)(rng.NextDouble() * 5.0 + 5.0)); // 5 a 10 segundos

        int rolID = ObtenerRolAleatorio(rng);
        int salaID = ObtenerSalaFromRol(rolID, rng);
        int tareaID = GetTarea(rolID, salaID);

        Debug.LogWarning("Para el NPC: " + npc.name + " | rolID: " + rolID + " | salaID: " + salaID + " | tareaID: " + tareaID);

        Vector3 destino = salaCoordenadas[salaID];
        yield return StartCoroutine(MoverNPC(npc, destino));

        yield return new WaitForSeconds((float)(rng.NextDouble() * 5.0 + 5.0));
        ActualizarEstadoTarea(tareaID);
    }

    int ObtenerRolAleatorio(System.Random rng)
    {
        List<int> roles = new List<int>();

        using (IDbConnection dbConnection = new SqliteConnection(dbPath))
        {
            dbConnection.Open();
            using (IDbCommand cmd = dbConnection.CreateCommand())
            {
                cmd.CommandText = $"SELECT ID FROM Roles";
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        roles.Add(reader.GetInt32(0));
                }
            }
        }

        if (roles.Count > 0)
        {
            int index = rng.Next(0, roles.Count);
            return roles[index];
        }
        else
        {
            Debug.LogError("No hay roles disponibles");
            return 1;
        }
    }

    int ObtenerSalaFromRol(int rolID, System.Random rng)
    {
        List<int> posiblesSalas = new List<int>();

        using (IDbConnection dbConnection = new SqliteConnection(dbPath))
        {
            dbConnection.Open();
            using (IDbCommand cmd = dbConnection.CreateCommand())
            {
                cmd.CommandText = $"SELECT Salas_ID FROM Tareas WHERE Rol_ID = {rolID}";
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        posiblesSalas.Add(reader.GetInt32(0));
                }
            }
        }

        if (posiblesSalas.Count == 0) return -1;
        int index = rng.Next(0, posiblesSalas.Count);
        return posiblesSalas[index];

    }

    int GetTarea(int rolID, int salaID)
    {
        using (IDbConnection dbConnection = new SqliteConnection(dbPath))
        {
            dbConnection.Open();
            using (IDbCommand cmd = dbConnection.CreateCommand())
            {
                cmd.CommandText = $"SELECT ID FROM Tareas WHERE Rol_ID = {rolID} AND Salas_ID = {salaID};";
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return reader.GetInt32(0);
                }
                return -1; // Si no se encuentra la tarea
            }
        }
    }

    void ActualizarEstadoTarea(int tareaID)
    {
        using (IDbConnection dbConnection = new SqliteConnection(dbPath))
        {
            dbConnection.Open();
            using (IDbCommand cmd = dbConnection.CreateCommand())
            {
                // Prepara tu UPDATE
                cmd.CommandText = $"UPDATE Tareas SET Estado = 1 WHERE ID = @id";
                // Mejor usa parámetro para evitar inyecciones y problemas de tipo:
                var param = cmd.CreateParameter();
                param.ParameterName = "@id";
                param.Value = tareaID;
                cmd.Parameters.Add(param);

                // Ejecuta realmente el comando
                int rowsAffected = cmd.ExecuteNonQuery();
                Debug.Log($"Estado de tarea actualizado a TRUE para ID: {tareaID}, filas afectadas: {rowsAffected}");
            }
            dbConnection.Close();
        }

    }

    IEnumerator MoverNPC(GameObject npc, Vector3 destino)
    {
        float speed = 2f;
        while (Vector3.Distance(npc.transform.position, destino) > 0.1f)
        {
            npc.transform.position = Vector3.MoveTowards(npc.transform.position, destino, speed * Time.deltaTime);
            yield return null;
        }
    }
}
