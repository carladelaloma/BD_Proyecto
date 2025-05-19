using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform[] spawnPoints;

    void Start()
    {
        foreach (Transform point in spawnPoints)
        {
            Instantiate(playerPrefab, point.position, Quaternion.identity);
        }
    }
}
