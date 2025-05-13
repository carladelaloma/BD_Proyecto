using UnityEngine;

public class VentTrigger : MonoBehaviour
{
    public int salaID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var manager = Object.FindFirstObjectByType<VentManager>();
            if (manager != null)
            {
                manager.TeleportFrom(salaID);
            }
        }
    }
}
