using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public SpriteRenderer hatSlot;
    public SpriteRenderer suitSlot;
    public string playerName;
    private UIManager uiManager;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        playerName = "Jugador_" + Random.Range(1000, 9999);
        uiManager.RegisterPlayer(playerName);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hat"))
        {
            hatSlot.sprite = other.GetComponent<SpriteRenderer>().sprite;
            Destroy(other.gameObject);
            uiManager.UpdatePlayerGear(playerName, "Sombrero");
        }
        else if (other.CompareTag("Suit"))
        {
            suitSlot.sprite = other.GetComponent<SpriteRenderer>().sprite;
            Destroy(other.gameObject);
            uiManager.UpdatePlayerGear(playerName, "Traje");
        }
    }
}
