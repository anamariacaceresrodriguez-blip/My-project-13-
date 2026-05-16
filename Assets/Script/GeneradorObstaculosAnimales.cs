using UnityEngine;

public sealed class GeneradorObstaculosAnimales : MonoBehaviour
{
    [Header("CONFIGURACIÓN DE ANIMALES")]
    public GameObject[] prefabsAnimales;
    public Transform jugador;
    public float distanciaGeneracion = 50f;

    [Header("ROTACIÓN")]
    [Tooltip("Si el animal mira hacia atrás después de aparecer, cambia esto a 180")]
    public float ajusteGiroY = 0f;

    [Header("TIEMPOS DE FASES (Segundos)")]
    public float tiempoSoloPeces = 20f;
    public float tiempoHastaTortugas = 40f;
    public float tiempoHastaDelfines = 60f;
    public float tiempoHastaCocodrilos = 80f;

    [Header("DIFICULTAD PROGRESIVA")]
    public float velocidadInicialAnimal = 2f;
    public float aumentoVelocidadPorSegundo = 0.05f;
    public float velocidadMaxima = 8f;
    private float velocidadActual;

    [Header("LÍMITES DEL RÍO")]
    public float limiteIzquierdo = -525f;
    public float limiteDerecho = -495f;
    public float alturaAgua = -268.5f; 

    private float proximoSpawn;
    private float cronometroEscena;

    void Start()
    {
        velocidadActual = velocidadInicialAnimal;
        cronometroEscena = 0f;
    }

    void Update()
    {
        if (jugador == null || prefabsAnimales.Length < 1) return;

        cronometroEscena += Time.deltaTime;

        if (velocidadActual < velocidadMaxima)
        {
            velocidadActual += aumentoVelocidadPorSegundo * Time.deltaTime;
        }

        if (Time.time > proximoSpawn)
        {
            GenerarAnimalSegunFase();
            proximoSpawn = Time.time + Random.Range(1.5f, 4.0f);
        }
    }

    void GenerarAnimalSegunFase()
    {
        int indiceMaximo = 0;
        if (cronometroEscena < tiempoSoloPeces) indiceMaximo = 1;
        else if (cronometroEscena < tiempoHastaTortugas) indiceMaximo = 2;
        else if (cronometroEscena < tiempoHastaDelfines) indiceMaximo = 3;
        else if (cronometroEscena < tiempoHastaCocodrilos) indiceMaximo = 4;
        else indiceMaximo = prefabsAnimales.Length;

        // Seguridad por si la lista no está llena
        indiceMaximo = Mathf.Clamp(indiceMaximo, 1, prefabsAnimales.Length);
        int indiceAleatorio = Random.Range(0, indiceMaximo);

        if (prefabsAnimales[indiceAleatorio] == null) return;

        // --- POSICIÓN ---
        float posX = Random.Range(limiteIzquierdo, limiteDerecho);
        Vector3 posicionSpawn = new Vector3(posX, alturaAgua, jugador.position.z + distanciaGeneracion);

        // --- INSTANCIACIÓN ---
        GameObject nuevoAnimal = Instantiate(prefabsAnimales[indiceAleatorio], posicionSpawn, Quaternion.identity);

       
        nuevoAnimal.transform.position = posicionSpawn;

      
        Vector3 puntoMirada = new Vector3(jugador.position.x, alturaAgua, jugador.position.z);
        nuevoAnimal.transform.LookAt(puntoMirada);

        // Ajuste manual 
        nuevoAnimal.transform.Rotate(0, ajusteGiroY, 0);

        // --- VELOCIDAD ---
        MovimientoAnimal mov = nuevoAnimal.GetComponent<MovimientoAnimal>();
        if (mov != null)
        {
            mov.rapidez = velocidadActual;
        }
    }
}