using UnityEngine;

public class PlayerSuitCollector : MonoBehaviour
{
    public Transform suitAttachPoint; // Pon aquí el Empty donde se colocará el traje
    private bool hasSuit = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasSuit) return;

        if (other.CompareTag("Suit"))
        {
            GameObject suit = other.gameObject;

            // Elimina física
            if (suit.TryGetComponent<Rigidbody2D>(out var rb))
                Destroy(rb);

            if (suit.TryGetComponent<Collider2D>(out var col))
                Destroy(col);

            if (suit.TryGetComponent<FloatingItem>(out var floatScript))
                Destroy(floatScript);

            // Lo pega visualmente a la posición indicada
            suit.transform.SetParent(suitAttachPoint);
            suit.transform.localPosition = Vector3.zero;
            suit.transform.localRotation = Quaternion.identity;

            hasSuit = true;
        }
    }
}
