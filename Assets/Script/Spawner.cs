using UnityEngine;

public class SpawnerBasura : MonoBehaviour
{
    [Header("Ritmo de la Lluvia")]
    [Tooltip("Cada cuántos segundos el Spawner intenta lanzar algo.")]
    public float intervaloIntento = 1f;

    [Tooltip("Probabilidad (0 a 100) de que realmente caiga un objeto en cada intento.")]
    [Range(0, 100)]
    public float probabilidadSpawn = 60f;

    [Header("Configuración de Objetos")]
    public GameObject[] basuras;
    public float rangoX = 5f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= intervaloIntento)
        {
            IntentarSpawn();
            timer = 0;
        }
    }

    void IntentarSpawn()
    {
        // Generamos un número al azar entre 0 y 100
        float suerte = Random.Range(0f, 100f);

        
        if (suerte <= probabilidadSpawn)
        {
            GenerarObjeto();
        }
    }

    void GenerarObjeto()
    {
        if (basuras.Length == 0) return;

        float xAleatorio = Random.Range(transform.position.x - rangoX, transform.position.x + rangoX);
        Vector3 posicionGeneracion = new Vector3(xAleatorio, transform.position.y, transform.position.z);

        int basuraIndex = Random.Range(0, basuras.Length);
        Instantiate(basuras[basuraIndex], posicionGeneracion, Quaternion.identity);
    }
}