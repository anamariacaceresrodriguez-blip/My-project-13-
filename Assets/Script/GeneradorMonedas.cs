using UnityEngine;

public class GeneradorMonedas : MonoBehaviour
{
    [Header("Configuración del Prefab")]
    public GameObject monedaPrefab;

    // Intervalo de 6 
    public float intervalo = 6.0f;

    // Aumentamos el rango a 10 
    public float rangoX = 10f;

    [Header("Referencia del Jugador")]
    public Transform jugador;
    public float distanciaAdelante = 40f;

    // Coordenadas fijas de tu río
    private float xCentroRio = -508.79f;
    private float alturaElevada = -260.0f;

    void Start()
    {
        if (monedaPrefab == null || jugador == null) return;

        InvokeRepeating("GenerarMoneda", 2f, intervalo);
    }

    void GenerarMoneda()
    {
        if (jugador == null) return;

        
        float xFinal = xCentroRio + Random.Range(-rangoX, rangoX);

        Vector3 posicionCreacion = new Vector3(xFinal, alturaElevada, jugador.position.z + distanciaAdelante);

        GameObject nuevaMoneda = Instantiate(monedaPrefab, posicionCreacion, Quaternion.identity);

        // Mantene la escala y rotación
        nuevaMoneda.transform.localScale = new Vector3(3.6f, 0.1f, 3.7f);
        nuevaMoneda.transform.rotation = Quaternion.Euler(-87.5f, -51.0f, 58.2f);

        Debug.Log("✅ Moneda generada en posición aleatoria: " + xFinal);
    }
}