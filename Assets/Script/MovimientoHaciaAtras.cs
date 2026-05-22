using UnityEngine;

public class MovimientoHaciaAtras : MonoBehaviour
{
    public float velocidad = 8f;

    void Update()
    {
        // Mueve el objeto hacia la cámara 
        transform.Translate(Vector3.back * velocidad * Time.deltaTime, Space.World);
    }
}