using UnityEngine;

public class GeneradorBasura : MonoBehaviour
{
    public Transform jugador;
    public GameObject[] prefabsBasura;

    [Header("Configuración del Río")]
    public float centroRioX = -509f;
    public float anchoRio = 30f;

    [Header("Ajuste de Altura")]
    public float alturaAgua = -268.5f;

    [Header("Ajustes de Spawn")]
    public float distanciaAdelante = 25f;
    
    public float tiempoGeneracion = 0.6f;
    public float velocidadBasura = 8f;

    void Start()
    {
        
        CancelInvoke();
        InvokeRepeating(nameof(GenerarBasura), 1f, tiempoGeneracion);
    }

    void GenerarBasura()
    {
        if (jugador == null) return;

        // Dispersión total en X
        float spawnX = Random.Range(centroRioX - anchoRio, centroRioX + anchoRio);
        float spawnZ = jugador.position.z + distanciaAdelante;
        float spawnY = alturaAgua;

        Vector3 posicionSpawn = new Vector3(spawnX, spawnY, spawnZ);

        int indice = Random.Range(0, prefabsBasura.Length);
        GameObject basura = Instantiate(
            prefabsBasura[indice],
            posicionSpawn,
            Quaternion.Euler(0, Random.Range(0, 360), 0)
        );

        MovimientoHaciaAtras mov = basura.GetComponent<MovimientoHaciaAtras>();
        if (mov == null) mov = basura.AddComponent<MovimientoHaciaAtras>();
        mov.velocidad = velocidadBasura;

        Destroy(basura, 15f);
    }
}