using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ControlVidas : MonoBehaviour
{
    [Header("Configuración")]
    public int vidasMaximas = 5;

    private int vidasActuales;

    [Header("Conexiones de UI")]
    public GameObject contenedorVidas;
    public GameObject prefabCorazon;

    public Sprite corazonLleno;
    public Sprite corazonVacio;

    private List<Image> listaImagenesCorazones = new List<Image>();

    void Start()
    {
        if (GestionPuntuacion.instancia != null)
        {
            vidasActuales = GestionPuntuacion.instancia.vidasGuardadas;
        }
        else
        {
            vidasActuales = vidasMaximas;
        }

        InicializarInterfaz();
        ActualizarUI();
    }

    void InicializarInterfaz()
    {
        foreach (Transform hijo in contenedorVidas.transform)
        {
            Destroy(hijo.gameObject);
        }

        listaImagenesCorazones.Clear();

        for (int i = 0; i < vidasMaximas; i++)
        {
            GameObject nuevoCorazon = Instantiate(prefabCorazon, contenedorVidas.transform);

            Image img = nuevoCorazon.GetComponent<Image>();

            if (img != null)
            {
                listaImagenesCorazones.Add(img);
                img.sprite = corazonLleno;
            }
            else
            {
                Debug.LogError("El prefabCorazon no tiene componente Image");
            }
        }
    }

    public void QuitarVida(int cantidad)
    {
        vidasActuales -= cantidad;

        vidasActuales = Mathf.Clamp(vidasActuales, 0, vidasMaximas);

        if (GestionPuntuacion.instancia != null)
        {
            GestionPuntuacion.instancia.vidasGuardadas = vidasActuales;
        }

        ActualizarUI();

        if (vidasActuales <= 0)
        {
            Debug.Log("¡Vidas agotadas!");
        }
    }

    public void EstablecerVidas(int vidas)
    {
        vidasActuales = Mathf.Clamp(vidas, 0, vidasMaximas);
        ActualizarUI();
    }

    void ActualizarUI()
    {
        for (int i = 0; i < listaImagenesCorazones.Count; i++)
        {
            if (listaImagenesCorazones[i] == null)
                continue;

            if (i < vidasActuales)
            {
                listaImagenesCorazones[i].sprite = corazonLleno;
            }
            else
            {
                listaImagenesCorazones[i].sprite = corazonVacio;
            }
        }
    }
}