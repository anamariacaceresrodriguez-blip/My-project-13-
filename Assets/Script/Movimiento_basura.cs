using UnityEngine;

public class MovimientoBasura : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float velocidad = Random.Range(1f, 3f);

        rb.linearVelocity = new Vector2(0, -velocidad);
    }
}