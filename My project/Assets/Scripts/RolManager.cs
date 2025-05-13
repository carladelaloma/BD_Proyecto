using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using Unity.VisualScripting;

public class RolManager : MonoBehaviour
{
    public Button vel_1;
    public Button pain_2;
    public Button suit_3;
    public Button smart_4;

    private string dbPath;
    private float cooldownTime = 1.5f;
    private float lastTeleportTime = -999f;

    void Start()
    {
        // Asociamos los eventos OnClick de los botones con la función InsertRolID pasando el Rol_ID correspondiente.
        vel_1.onClick.AddListener(OnButton1Click); // Rol_ID = 1
        pain_2.onClick.AddListener(() => InsertRolID(2)); // Rol_ID = 2
        suit_3.onClick.AddListener(() => InsertRolID(3)); // Rol_ID = 3
        smart_4.onClick.AddListener(() => InsertRolID(4)); // Rol_ID = 4
    }
    void OnButton1Click()
    {
        InsertRolID(1);
    }

    void InsertRolID(int rolID)
    {
        if (Time.time < lastTeleportTime + cooldownTime)
            return;

        dbPath = "URI=file:" + Application.dataPath + "/Plugins/among_db.sqlite";

        using (IDbConnection dbConnection = new SqliteConnection(dbPath))
        {
            dbConnection.Open();
            using (IDbCommand cmd = dbConnection.CreateCommand())
            {
                cmd.CommandText = $"INSERT INTO Personajes (Rol_ID) VALUES ({rolID})";
                cmd.ExecuteNonQuery();
            }
        }

        lastTeleportTime = Time.time;
    }
    }