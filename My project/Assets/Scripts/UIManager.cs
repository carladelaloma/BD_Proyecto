using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public GameObject tableRowPrefab;
    public Transform tableContent;

    private Dictionary<string, Text> playerRows = new Dictionary<string, Text>();

    public void RegisterPlayer(string name)
    {
        GameObject row = Instantiate(tableRowPrefab, tableContent);
        Text rowText = row.GetComponent<Text>();
        rowText.text = $"{name}: Ninguno";
        playerRows[name] = rowText;
    }

    public void UpdatePlayerGear(string name, string item)
    {
        if (playerRows.ContainsKey(name))
        {
            string current = playerRows[name].text;
            if (!current.Contains(item))
            {
                playerRows[name].text = current.Replace("Ninguno", "").Trim();
                playerRows[name].text += item + " ";
            }
        }
    }
}
