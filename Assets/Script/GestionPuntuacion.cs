using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GestionPuntuacion : MonoBehaviour
{
    public static GestionPuntuacion instancia;

    [Header("Base de Datos")]
    public int puntosMinijuego = 0;
    public int monedasDelRio = 0;
    public int vidasGuardadas = 5; 

    private TextMeshProUGUI textoPuntosUI;
    private TextMeshProUGUI textoMonedasUI;

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
        StartCoroutine(EsperarYActualizar());
    }

    IEnumerator EsperarYActualizar()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject objPuntos = GameObject.FindGameObjectWithTag("Puntos");
        if (objPuntos != null) textoPuntosUI = objPuntos.GetComponent<TextMeshProUGUI>();

        GameObject objMonedas = GameObject.FindGameObjectWithTag("ContadorMonedas");
        if (objMonedas != null) textoMonedasUI = objMonedas.GetComponent<TextMeshProUGUI>();

        ActualizarInterfazGrafica();
    }

    public void ActualizarInterfazGrafica()
    {
        if (textoPuntosUI != null) textoPuntosUI.text = puntosMinijuego.ToString();
        if (textoMonedasUI != null) textoMonedasUI.text = monedasDelRio.ToString();
    }

    public void GanarPuntos(int cantidad) { puntosMinijuego += cantidad; ActualizarInterfazGrafica(); }
    public void RecogerMonedaRio()
    {
        monedasDelRio += 1;
        ActualizarInterfazGrafica();

       
        if (LogrosManager.Instancia != null)
        {
            LogrosManager.Instancia.ChequearMonedas(monedasDelRio);
        }
    }


    public void ResetearTodo()
    {
        puntosMinijuego = 0;
        monedasDelRio = 0;
        vidasGuardadas = 5;
        ActualizarInterfazGrafica();
    }
}