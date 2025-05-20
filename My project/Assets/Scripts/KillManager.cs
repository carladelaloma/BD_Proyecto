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

    void Start()
    {
        dbPath = "URI=file:" + Application.dataPath + "/Plugins/among_db.sqlite";
        killButton.onClick.AddListener(TryKill);    
        killButton.gameObject.SetActive(false);
    }

    void TryKill()
    {
        GameObject[] currentNPCs = GameObject.FindGameObjectsWithTag("NPC");

        Transform closestTarget = null;
        float minDistance = float.MaxValue;

        foreach (GameObject npc in currentNPCs)
        {
            if (npc == null)
                continue;

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
            int salaID = ObtenerSalaMasCercana(closestTarget.position);
            InsertMuerte(playerName, victimName, salaID);
            Destroy(closestTarget.gameObject);
        }
        else
        {
            Debug.Log("No hay nadie suficientemente cerca para matar.");
        }
    }

    int ObtenerSalaMasCercana(Vector3 victimaPos)
    {
        GameObject[] trampillas = GameObject.FindGameObjectsWithTag("Vent");
        float minDist = float.MaxValue;
        int salaID = -1;

        foreach (var trampilla in trampillas)
        {
            float dist = Vector3.Distance(victimaPos, trampilla.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                VentTrigger vent = trampilla.GetComponent<VentTrigger>();
                if (vent != null)
                {
                    salaID = vent.salaID;
                }
            }
        }

        Debug.Log($"Sala más cercana a la víctima (por trampilla): {salaID}");
        return salaID;
    }

    void InsertMuerte(string asesino, string victima, int salaID)
    {
        using (IDbConnection dbConnection = new SqliteConnection(dbPath))
        {
            dbConnection.Open();
            using (IDbCommand cmd = dbConnection.CreateCommand())
            {
                cmd.CommandText = $"INSERT INTO MUERTES (Asesino, Victima, Salas_ID) VALUES (@asesino, @victima, @sala)";
                cmd.Parameters.Add(new SqliteParameter("@asesino", asesino));
                cmd.Parameters.Add(new SqliteParameter("@victima", victima));
                cmd.Parameters.Add(new SqliteParameter("@sala", salaID));
                cmd.ExecuteNonQuery();
            }
        }

        Debug.Log($"Muerte registrada en la base de datos: {asesino} mató a {victima} en sala {salaID}");
    }


    void Update()
    {
        bool anyNear = false;
        GameObject[] currentNPCs = GameObject.FindGameObjectsWithTag("NPC");

        foreach (GameObject npc in currentNPCs)
        {
            if (npc == null) continue;
            float distance = Vector3.Distance(playerTransform.position, npc.transform.position);
            if (distance < killDistance)
            {
                anyNear = true;
                break;
            }
        }

        // activar solo si hay NPC en rango, ocultar en caso contrario
        killButton.gameObject.SetActive(anyNear);
    }
}