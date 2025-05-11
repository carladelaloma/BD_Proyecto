using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;

public class VentManager : MonoBehaviour
{
    private string dbPath;
    private Dictionary<int, Transform> trampillas = new Dictionary<int, Transform>();
    private float cooldownTime = 1.5f;
    private float lastTeleportTime = -999f;

    void Start()
    {
        dbPath = "URI=file:" + Application.dataPath + "/Plugins/among_db.sqlite";

        foreach (VentTrigger vent in Object.FindObjectsByType<VentTrigger>(FindObjectsSortMode.None))
        {
            trampillas[vent.salaID] = vent.transform;
        }
    }

    public void TeleportFrom(int salaOrigen)
    {
        if (Time.time < lastTeleportTime + cooldownTime)
            return;

        int salaDestino = GetSalaConectada(salaOrigen);

        if (trampillas.ContainsKey(salaDestino))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = trampillas[salaDestino].position;
            lastTeleportTime = Time.time;
        }
    }

    private int GetSalaConectada(int origen)
    {
        using (IDbConnection dbConnection = new SqliteConnection(dbPath))
        {
            dbConnection.Open();
            using (IDbCommand cmd = dbConnection.CreateCommand())
            {
                cmd.CommandText = $"SELECT ID_2 FROM Salas_Salas WHERE ID_1 = {origen} LIMIT 1";
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return reader.GetInt32(0);
                }
            }
        }

        Debug.LogWarning("No se encontró conexión para la trampilla " + origen);
        return origen;
    }
}
