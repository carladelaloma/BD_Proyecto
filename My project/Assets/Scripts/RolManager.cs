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

    public GameObject uiCanva;

    private string dbPath;
    private float cooldownTime = 1.5f;
    private float lastTeleportTime = -999f;

    void Start()
    {
        // Asociamos los eventos OnClick de los botones con la función InsertRolID pasando el Rol_ID correspondiente.
        vel_1.onClick.AddListener(OnButton1Click); // Rol_ID = 1
        pain_2.onClick.AddListener(OnButton2Click); // Rol_ID = 2
        suit_3.onClick.AddListener(OnButton3Click); // Rol_ID = 3
        smart_4.onClick.AddListener(OnButton4Click); // Rol_ID = 4
    }
    void OnButton1Click()
    {
        Debug.Log("Botón 1 presionado");  // Imprime por consola
        InsertRolID(1);
        DisableButtons();  // Desactiva los botones
    }

    void OnButton2Click()
    {
        Debug.Log("Botón 2 presionado");  // Imprime por consola
        InsertRolID(2);
        DisableButtons();  // Desactiva los botones
    }

    void OnButton3Click()
    {
        Debug.Log("Botón 3 presionado");  // Imprime por consola
        InsertRolID(3);
        DisableButtons();  // Desactiva los botones
    }

    void OnButton4Click()
    {
        Debug.Log("Botón 4 presionado");  // Imprime por consola
        InsertRolID(4);
        DisableButtons();  // Desactiva los botones
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

    // Desactiva todos los botones y oculta el panel UI
    void DisableButtons()
    {
        // Desactiva los botones para que no se puedan pulsar nuevamente
        vel_1.interactable = false;
        pain_2.interactable = false;
        suit_3.interactable = false;
        smart_4.interactable = false;

        // Si deseas desactivar toda la UI, puedes ocultar el panel completo
        if (uiCanva != null)
        {
            uiCanva.SetActive(false);  // Esto desactiva todo el panel que contiene los botones
        }
    }
}