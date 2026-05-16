using UnityEngine;

public class MovimientoJugadorCompleto : MonoBehaviour
{
    // Velocidad 
    public float velocidadAvance = 5f;

    void Update()
    {
        // Movemo el objeto hacia adelante 
        transform.Translate(Vector3.forward * velocidadAvance * Time.deltaTime);
    }
}