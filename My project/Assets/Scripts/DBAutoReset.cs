using UnityEngine;
using System.IO;

public class DBAutoReset : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        ResetearBaseDeDatos();
    }

    private void ResetearBaseDeDatos()
    {
        string originalPath = Application.dataPath + "/Plugins/among_db_backup.sqlite";
        string activePath = Application.dataPath + "/Plugins/among_db.sqlite";

        if (File.Exists(originalPath))
        {
            File.Copy(originalPath, activePath, true);
            Debug.Log("[DBAutoReset] Base de datos restaurada a su estado original.");
        }
        else
        {
            Debug.LogError("[DBAutoReset] No se encontró el archivo de respaldo: " + originalPath);
        }
    }
}
