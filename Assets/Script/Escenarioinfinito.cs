using UnityEngine;

public class EscenarioInfinito : MonoBehaviour
{
    public float longitudModulo = 281.5f;

    
    public float anticipacion = 3f;

    private Transform transformJugador;
    private float alturaFija;

    void Start()
    {
        GameObject jugador = GameObject.Find("Jugador_Completo");
        if (jugador != null)
        {
            transformJugador = jugador.transform;
        }

        // Guarda la altura 
        alturaFija = transform.position.y;
    }

    void Update()
    {
        if (transformJugador == null) return;

        
        if (transformJugador.position.z > transform.position.z + (longitudModulo / anticipacion))
        {
            Vector3 nuevaPos = transform.position;

            nuevaPos.z += longitudModulo * 2f;

            // Mantenemos la altura 
            nuevaPos.y = alturaFija;

            transform.position = nuevaPos;

            Debug.Log("¡Río adelante!");
        }
    }
}