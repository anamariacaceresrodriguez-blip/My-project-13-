using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class ControlCapibara : MonoBehaviour
{
    [Header("CONFIGURACIÓN CRÍTICA")]
    public BoxCollider colliderCanoa;
    public Animator animadorCapibara;

    [Header("SISTEMA DE VIDAS (CORAZONES)")]
    private int vidasActuales;
    public float tiempoInvencible = 1.5f;
    private float proximoDañoPosible = 0f;
    private ControlVidas sistemaVidasVisual;

    [Header("SONIDOS")]
    public AudioSource fuenteEfectos;
    public AudioClip sonidoSalto;
    public AudioClip sonidoRecoger;
    public AudioClip sonidoChoque;
    public AudioClip sonidoMoneda;

    [Header("MOVIMIENTO")]
    public float velocidadAvance = 10f;
    public float velocidadLateral = 8f;
    public float fuerzaSalto = 12f;

    [Header("AJUSTES DE SALTO")]
    public float tiempoEntreSaltos = 0.1f; 
    private float proximoSaltoDisponible = 0f;

    [Header("PROGRESO (BASURA)")]
    public int progresoBasura = 0;
    public int metaBasura = 25;
    public Text textoInterfaz;
    public Slider barraProgreso;

    // --- VARIABLES DE NAVEGACIÓN ---
    public static bool volverDeMiniJuego = false;
    public static Vector3 posicionGuardada;
    public static bool guardarPosicion = false;

    private Rigidbody rb;
    private bool estaPausado = false;
    private Vector3 centroOriginal;
    private Vector3 tamañoOriginal;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        sistemaVidasVisual = Object.FindFirstObjectByType<ControlVidas>();

        // CARGAR VIDAS GUARDADAS
        if (GestionPuntuacion.instancia != null)
        {
            vidasActuales = GestionPuntuacion.instancia.vidasGuardadas;
        }
        else
        {
            vidasActuales = 5;
        }

        // ACTUALIZAR UI
        if (sistemaVidasVisual != null)
        {
            sistemaVidasVisual.EstablecerVidas(vidasActuales);
        }

        if (volverDeMiniJuego)
        {
            transform.position = posicionGuardada;
        }

        // CORAZONES


        if (colliderCanoa != null)
        {
            centroOriginal = colliderCanoa.center;
            tamañoOriginal = colliderCanoa.size;
        }
        Time.timeScale = 1;

        if (barraProgreso != null)
        {
            barraProgreso.maxValue = metaBasura;
            barraProgreso.value = progresoBasura;
        }
        ActualizarUI();
    }


    IEnumerator ReiniciarConRetraso()
    {
        yield return new WaitForSeconds(1.5f); // Espera 1.5 segundos para que la API responda
        SceneManager.LoadScene("TuEscena");
    }
    // --- FUNCIÓN DE SONIDO ---
    void ReproducirSonido(AudioClip clip)
    {
        if (AudioManager.instancia != null)
        {
            AudioManager.instancia.ReproducirEfecto(clip);
        }
        else if (fuenteEfectos != null && clip != null)
        {
            fuenteEfectos.PlayOneShot(clip);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) TogglePausa();
        if (estaPausado) return;

      
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Time.time >= proximoSaltoDisponible)
            {
                proximoSaltoDisponible = Time.time + tiempoEntreSaltos;

                // respuesta inmediata
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

                rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, fuerzaSalto, rb.linearVelocity.z);

                ReproducirSonido(sonidoSalto);

                if (animadorCapibara != null)
                {
                    animadorCapibara.Play("Salto", 0, 0f);
                    animadorCapibara.SetInteger("States", 0);
                }
            }
        }

        ManejarAgachado();
    }

    void ManejarAgachado()
    {
        bool presionandoS = Input.GetKey(KeyCode.S);

        if (colliderCanoa != null)
        {
            if (presionandoS)
            {
                float nuevoTamañoY = tamañoOriginal.y * 0.5f;
                colliderCanoa.size = new Vector3(tamañoOriginal.x, nuevoTamañoY, tamañoOriginal.z);
                float diferencia = (tamañoOriginal.y - nuevoTamañoY) / 2f;
                colliderCanoa.center = new Vector3(centroOriginal.x, centroOriginal.y - diferencia, centroOriginal.z);

                if (animadorCapibara != null)
                {
                    animadorCapibara.SetInteger("States", 0);
                    animadorCapibara.Play("Agachado", 0);
                }
            }
            else
            {
                colliderCanoa.size = tamañoOriginal;
                colliderCanoa.center = centroOriginal;
            }
        }
    }

    void FixedUpdate()
    {
        if (estaPausado) return;

        float movHorizontal = 0;
        if (Input.GetKey(KeyCode.A)) movHorizontal = -1;
        if (Input.GetKey(KeyCode.D)) movHorizontal = 1;

        rb.linearVelocity = new Vector3(movHorizontal * velocidadLateral, rb.linearVelocity.y, velocidadAvance);

        if (animadorCapibara != null && !Input.GetKey(KeyCode.S))
        {
            animadorCapibara.SetInteger("States", 2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Lógica para la Basura 
        if (other.CompareTag("Recolectable"))
        {
            // --- PRIMERO CAPTURAMOS EL NOMBRE ANTES DE QUE SE DESTRUYA ---
            if (ApiBootstrapper.Instancia != null && other.gameObject != null)
            {
                string tipoBasura = other.gameObject.name.Replace("(Clone)", "").Trim();
                ApiBootstrapper.Instancia.basuraAcumuladaEnRio.Add(tipoBasura);
                Debug.Log($">>> [{tipoBasura}] guardado en la bolsa virtual de Unity.");
            }

            // Ahora sí, llamamos a tus funciones normales
            RecogerBasura(other.gameObject);
            ReproducirSonido(sonidoRecoger);
        }
        // ---  MONEDAS ---
        else if (other.CompareTag("ContadorMonedas"))
        {
            if (GestionPuntuacion.instancia != null)
            {
                GestionPuntuacion.instancia.RecogerMonedaRio();
            }

            ReproducirSonido(sonidoMoneda);
            Destroy(other.gameObject);
        }
        //  Obstáculos 
        else if (other.CompareTag("Obstaculo"))
        {
            if (Time.time >= proximoDañoPosible)
            {
                ProcesarDaño();
                ReproducirSonido(sonidoChoque);
                Destroy(other.gameObject);
                proximoDañoPosible = Time.time + tiempoInvencible;
            }
        }
    }

    void ProcesarDaño()
    {
        vidasActuales--;

        if (GestionPuntuacion.instancia != null)
        {
            GestionPuntuacion.instancia.vidasGuardadas = vidasActuales;
        }

        if (sistemaVidasVisual != null)
            sistemaVidasVisual.QuitarVida(1);

        if (vidasActuales <= 0)
        {
           
            StartCoroutine(GuardarYReiniciar());
        }
    }

    IEnumerator GuardarYReiniciar()
    {
        Debug.Log(">>> Muerte natural del Capibara: Enviando datos finales a Somee...");

        int monedasFinales = 0;
        int puntosFinales = 0;

        if (GestionPuntuacion.instancia != null)
        {
            monedasFinales = GestionPuntuacion.instancia.monedasDelRio;
            puntosFinales = GestionPuntuacion.instancia.puntosMinijuego;
        }

        if (ApiBootstrapper.Instancia != null)
        {
            
            ApiBootstrapper.Instancia.GuardarPartidaFinalizada(monedasFinales, puntosFinales, progresoBasura);
        }

        yield return new WaitForSeconds(1.0f);

        if (GestionPuntuacion.instancia != null)
        {
            GestionPuntuacion.instancia.ResetearTodo();
        }

        progresoBasura = 0;
        volverDeMiniJuego = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void RecogerBasura(GameObject basura)
    {
        // Validamos que el objeto no sea nulo antes de destruirlo
        if (basura != null)
        {
            Destroy(basura);
        }

        progresoBasura++;
        if (fuenteEfectos != null && sonidoRecoger != null) fuenteEfectos.PlayOneShot(sonidoRecoger);

        ActualizarBarra();
        ActualizarUI();

        if (progresoBasura >= metaBasura)
        {
            posicionGuardada = transform.position;
            volverDeMiniJuego = true;
            IrAMiniJuego();
        }
    }


    void ActualizarBarra()
    {
       
        if (barraProgreso != null)
        {
            barraProgreso.value = progresoBasura;
        }
    }

    void ActualizarUI()
    {
       
        if (textoInterfaz != null)
        {
            textoInterfaz.text = "Items: " + progresoBasura + " / " + metaBasura;
        }
    }

    void IrAMiniJuego()
    {
     

        StartCoroutine(EsperarYCambiarEscena());
    }

    private System.Collections.IEnumerator EsperarYCambiarEscena()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("2d_mini");
    }
    void TogglePausa() { estaPausado = !estaPausado; Time.timeScale = estaPausado ? 0 : 1; }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * 0.5f);
    }
}