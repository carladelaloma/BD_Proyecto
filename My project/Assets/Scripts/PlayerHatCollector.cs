using UnityEngine;

public class PlayerHatCollector : MonoBehaviour
{
    public Transform hatAttachPoint; // Objeto vacío en la cabeza
    private bool hasHat = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHat) return; // Ya tiene un sombrero, no hace nada

        if (other.CompareTag("Hat"))
        {
            // Detach del mundo
            GameObject hat = other.gameObject;

            // Detenemos toda física y scripts
            if (hat.TryGetComponent<Rigidbody2D>(out var rb))
                Destroy(rb);

            if (hat.TryGetComponent<Collider2D>(out var col))
                Destroy(col);

            if (hat.TryGetComponent<FloatingItem>(out var floatScript))
                Destroy(floatScript);

            // Lo hacemos hijo del jugador
            hat.transform.SetParent(hatAttachPoint);
            hat.transform.localPosition = Vector3.zero;
            hat.transform.localRotation = Quaternion.identity;

            hasHat = true; // No permite pegar más
        }
    }
}