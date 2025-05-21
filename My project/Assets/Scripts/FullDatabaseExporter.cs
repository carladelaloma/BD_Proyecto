using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System;

public class FullDatabaseExporter : MonoBehaviour
{
    private string dbPath;

    void Start()
    {
        dbPath = "URI=file:" + Application.dataPath + "/Plugins/among_db.sqlite";
    }

    [ContextMenu("Exportar todas las tablas")]
    public void ExportarTodo()
    {
        List<string> tablas = ObtenerNombresDeTablas();

        foreach (string tabla in tablas)
        {
            List<Dictionary<string, object>> datos = ObtenerFilasDeTabla(tabla);
            List<SerializableRow> serializables = new List<SerializableRow>();
            foreach (var fila in datos)
                serializables.Add(new SerializableRow(fila));

            GuardarComoJSON(tabla, serializables);
            GuardarComoXML(tabla, serializables);
        }

        Debug.Log("✅ Exportación completa de todas las tablas.");
    }

    List<string> ObtenerNombresDeTablas()
    {
        List<string> tablas = new List<string>();

        using (IDbConnection conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%'";
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        tablas.Add(reader.GetString(0));
                }
            }
        }

        return tablas;
    }

    List<Dictionary<string, object>> ObtenerFilasDeTabla(string tabla)
    {
        List<Dictionary<string, object>> filas = new List<Dictionary<string, object>>();

        using (IDbConnection conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"SELECT * FROM {tabla}";
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object> fila = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string campo = reader.GetName(i);
                            object valor = reader.IsDBNull(i) ? null : reader.GetValue(i);
                            fila[campo] = valor;
                        }

                        filas.Add(fila);
                    }
                }
            }
        }

        return filas;
    }

    void GuardarComoJSON(string tabla, List<SerializableRow> datos)
    {
        string json = JsonUtility.ToJson(new SerializableRowList(datos), true);
        string ruta = Path.Combine(Application.persistentDataPath, tabla + ".json");
        File.WriteAllText(ruta, json);
        Debug.Log("✅ Exportado JSON: " + ruta);
    }

    void GuardarComoXML(string tabla, List<SerializableRow> datos)
    {
        string ruta = Path.Combine(Application.persistentDataPath, tabla + ".xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<SerializableRow>));
        using (FileStream stream = new FileStream(ruta, FileMode.Create))
        {
            serializer.Serialize(stream, datos);
        }

        Debug.Log("✅ Exportado XML: " + ruta);
    }
}

[Serializable]
public class SerializableRow
{
    public List<string> claves = new List<string>();
    public List<string> valores = new List<string>();

    public SerializableRow() { }

    public SerializableRow(Dictionary<string, object> data)
    {
        foreach (var kv in data)
        {
            claves.Add(kv.Key);
            valores.Add(kv.Value?.ToString() ?? "");
        }
    }
}

[Serializable]
public class SerializableRowList
{
    public List<SerializableRow> filas;

    public SerializableRowList(List<SerializableRow> filas)
    {
        this.filas = filas;
    }
}
