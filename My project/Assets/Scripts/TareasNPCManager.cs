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
        yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));

        int rolID = ObtenerRolAleatorio();
        int salaID = ObtenerSalaFromRol(rolID);    
        int tareaID = GetTarea(rolID, salaID);

        Debug.LogWarning("Para el ncp: " + npc + "rolid: " + rolID + "salaID:" + salaID + "tarea id: " + tareaID);
        // Mover al NPC a la sala correspondiente
        Vector3 destino = salaCoordenadas[salaID];
        yield return StartCoroutine(MoverNPC(npc, destino));

        // Espera otros 5-10 segundos antes de actualizar el estado
        yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
        ActualizarEstadoTarea(tareaID);
    }

    int ObtenerRolAleatorio()
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
                    if (reader.Read())
                        roles.Add(reader.GetInt32(0));
                }
            }
        }

        if (roles.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, roles.Count);
            return roles[index];
        }
        else
        {
            Debug.LogError("No hay roles disponibles");
            return 1; // Valor por defecto
        }
    }

    int ObtenerSalaFromRol(int rolID)
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
                    if (reader.Read()){
                        posiblesSalas.Add(reader.GetInt32(0));
                        
                        if (posiblesSalas.Count == 1){
                            return posiblesSalas[0]; // Si solo hay una sala, la devuelve directamente
                        }
                        else{
                            int index = UnityEngine.Random.Range(0, posiblesSalas.Count);
                            return posiblesSalas[index];
                        }
                    }
                    return -1; // Si no se encuentra la sala

                }
            }
        }

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
                cmd.CommandText = $"UPDATE Tareas SET Estado = true WHERE ID = {tareaID}";
                Debug.Log("Estado de tarea actualizado a TRUE para ID: " + tareaID);
            }
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
