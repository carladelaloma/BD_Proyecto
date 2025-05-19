using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float floatRange = 0.5f;

    private Vector3 startPos;
    private float offset;

    void Start()
    {
        startPos = transform.position;
        offset = Random.Range(0f, Mathf.PI * 2);
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * floatSpeed + offset) * floatRange;
        transform.position = new Vector3(startPos.x, startPos.y + y, startPos.z);
    }
}
