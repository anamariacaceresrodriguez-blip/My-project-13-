using UnityEngine;
using TMPro;
using System.Collections;

public class ControladorInterfaz : MonoBehaviour
{
    public static ControladorInterfaz instancia;

    [Header("Referencias de UI")]
    public GameObject objetoCartel; // El panel que contiene el fondo y el texto
    public TextMeshProUGUI textoMensaje; 

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MostrarCartel(string texto)
    {
        if (objetoCartel == null || textoMensaje == null)
        {
            Debug.LogError("⚠️ ControladorInterfaz: ¡Faltan referencias en el Inspector!");
            return;
        }

        textoMensaje.text = texto;
        objetoCartel.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(OcultarDespuesDeTiempo(4f));
    }

    IEnumerator OcultarDespuesDeTiempo(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        objetoCartel.SetActive(false);
    }
}