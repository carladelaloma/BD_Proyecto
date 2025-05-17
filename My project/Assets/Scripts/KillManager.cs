using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;

public class KillManager : MonoBehaviour
{
    public Button killButton;
    public float killDistance = 2f;
    public Transform playerTransform;
    public string playerName => playerTransform.name;

    private string dbPath;
    private GameObject[] npcs;

    void Start()
    {
        dbPath = "URI=file:" + Application.dataPath + "/Plugins/among_db.sqlite";
        killButton.onClick.AddListener(TryKill);

        // Buscar todos los NPCs con el tag "NPC"
        npcs = GameObject.FindGameObjectsWithTag("NPC");
    }

    void TryKill()
    {
        Transform closestTarget = null;
        float minDistance = float.MaxValue;

        foreach (GameObject npc in npcs)
        {
            float distance = Vector3.Distance(playerTransform.position, npc.transform.position);

            if (distance < killDistance && distance < minDistance)
            {
                minDistance = distance;
                closestTarget = npc.transform;
            }
        }

        if (closestTarget != null)
        {
            string victimName = closestTarget.name;
            Debug.Log(playerName + " mató a " + victimName);
            InsertMuerte(playerName, victimName);
            Destroy(closestTarget.gameObject);
        }
        else
        {
            Debug.Log("No hay nadie suficientemente cerca para matar.");
        }
    }

    void InsertMuerte(string asesino, string victima)
    {
        using (IDbConnection dbConnection = new SqliteConnection(dbPath))
        {
            dbConnection.Open();
            using (IDbCommand cmd = dbConnection.CreateCommand())
            {
                cmd.CommandText = $"INSERT INTO MUERTES (Asesino, Victima) VALUES (@asesino, @victima)";
                cmd.Parameters.Add(new SqliteParameter("@asesino", asesino));
                cmd.Parameters.Add(new SqliteParameter("@victima", victima));
                cmd.ExecuteNonQuery();
                Debug.Log("Muerte registrada en la base de datos correctamente.");
            }
        }
    }
}


