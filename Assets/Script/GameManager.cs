using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    [Header("Configuración de Puntos")]
    public int puntosTotales = 0;
    public int puntosMaximos = 25;

    [Header("Referencias UI")]
    public TextMeshProUGUI textoPuntos;
    public Image barraBasura;

    [Header("Configuración de Sonidos")]
    public AudioSource fuenteAudio;
    public AudioClip sonidoAcierto;
    public AudioClip sonidoFallo;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable() { SceneManager.sceneLoaded += AlCargarEscena; }
    void OnDisable() { SceneManager.sceneLoaded -= AlCargarEscena; }

    void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        EncontrarReferenciasUI();
    }

    void EncontrarReferenciasUI()
    {
        GameObject objTexto = GameObject.FindGameObjectWithTag("Puntos");
        if (objTexto != null) textoPuntos = objTexto.GetComponent<TextMeshProUGUI>();

        GameObject objBarra = GameObject.Find("BarraBasura");
        if (objBarra != null) barraBasura = objBarra.GetComponent<Image>();

        ActualizarTodo();
    }
    public void Sumar()
    {
        puntosTotales++;

        if (GestionPuntuacion.instancia != null)
        {
            GestionPuntuacion.instancia.GanarPuntos(1);
        }

        if (LogrosManager.Instancia != null)
        {
            LogrosManager.Instancia.ChequearPuntosMinijuego(puntosTotales);
        }

        ActualizarTodo();
        ReproducirSonido(sonidoAcierto);

        // --- AQUÍ SÍ ES EL MOMENTO REAL DE GUARDAR ---
        if (puntosTotales >= puntosMaximos)
        {
            Debug.Log(">>> ¡MINIJUEGO COMPLETADO! Enviando puntaje final real: " + this.puntosTotales);

            if (ApiBootstrapper.Instancia != null)
            {
                // Aseguramos que el script global se actualice con el valor real de esta escena
                if (GestionPuntuacion.instancia != null)
                {
                    GestionPuntuacion.instancia.puntosMinijuego = this.puntosTotales;
                }

                int monedas = (GestionPuntuacion.instancia != null) ? GestionPuntuacion.instancia.monedasDelRio : 0;
                int puntosAEnviar = this.puntosTotales;

                // Esta es la única y verdadera llamada con los datos completos
                ApiBootstrapper.Instancia.GuardarPartidaFinalizada(monedas, puntosAEnviar, puntosAEnviar);
            }

            SceneManager.LoadScene("2d_mini");
        }
    }
    public void Restar()
    {
        if (puntosTotales > 0)
        {
            puntosTotales--;
            ActualizarTodo();
            ReproducirSonido(sonidoFallo);
        }
    }

    public void ActualizarTodo()
    {
        if (textoPuntos != null) textoPuntos.text = "Puntos: " + puntosTotales;

        if (barraBasura != null)
        {
            barraBasura.fillAmount = (float)puntosTotales / (float)puntosMaximos;
        }
    }

    void ReproducirSonido(AudioClip clip)
    {
        if (fuenteAudio != null && clip != null) fuenteAudio.PlayOneShot(clip);
    }
}