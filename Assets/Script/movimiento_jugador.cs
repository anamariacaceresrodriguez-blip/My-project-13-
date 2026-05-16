using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidad = 10f;

    void Update()
    {
        float movimiento = Input.GetAxis("Horizontal");
        Debug.Log("Movimiento: " + movimiento);

        transform.Translate(Vector3.right * movimiento * velocidad * Time.deltaTime);
    }
}