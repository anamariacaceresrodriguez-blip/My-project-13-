using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogrosManager : MonoBehaviour
{
    public static LogrosManager Instancia { get; private set; }

    [Header("UI del Logro")]
    public GameObject panelNotificacion;
    public TextMeshProUGUI textoNotificacion;

    private bool mision1Superada = false;
    private bool mision2Superada = false;
    private bool mision3Superada = false;
    private bool mision4Superada = false;
    private bool mision5Superada = false;

   
    public List<string> logrosObtenidosEnEstaPartida = new List<string>();

    void Awake()
    {
       
        if (Instancia == null)
        {
            Instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (panelNotificacion != null) panelNotificacion.SetActive(false);
    }

    public void ChequearMonedas(int cantidad)
    {
        if (cantidad >= 10 && !mision1Superada)
        {
            mision1Superada = true;
            ProcesarLogroObtenido("Misión 1 Superada: ¡Eco-Navegante Inicial!", "Misión 1: 10 Monedas Juntadas");
        }
        if (cantidad >= 30 && !mision3Superada)
        {
            mision3Superada = true;
            ProcesarLogroObtenido("Misión 3 Superada: ¡Tesoro del Río!", "Misión 3: 30 Monedas Acumuladas");
        }
    }

    public void ChequearBasura(int cantidad)
    {
        if (cantidad >= 5 && !mision2Superada)
        {
            mision2Superada = true;
            ProcesarLogroObtenido("Misión 2 Superada: ¡Limpiando el Amazonas!", "Misión 2: 5 Basuras Recogidas");
        }
        if (cantidad >= 15 && !mision4Superada)
        {
            mision4Superada = true;
            ProcesarLogroObtenido("Misión 4 Superada: ¡Defensor del Agua!", "Misión 4: 15 Basuras Recogidas");
        }
    }

    public void ChequearPuntosMinijuego(int cantidad)
    {
        if (cantidad >= 25 && !mision5Superada)
        {
            mision5Superada = true;
            ProcesarLogroObtenido("Misión 5 Superada: ¡Puntuación Perfecta!", "Misión 5: 25 Puntos en Minijuego");
        }
    }

    private void ProcesarLogroObtenido(string mensajeUI, string nombreLogroSQL)
    {
        if (panelNotificacion != null && textoNotificacion != null)
        {
            textoNotificacion.text = mensajeUI;
            panelNotificacion.SetActive(true);
            
            Invoke("OcultarPanel", 4f);
        }

        if (!logrosObtenidosEnEstaPartida.Contains(nombreLogroSQL))
        {
            logrosObtenidosEnEstaPartida.Add(nombreLogroSQL);
        }
    }

    void OcultarPanel()
    {
        if (panelNotificacion != null) panelNotificacion.SetActive(false);
    }

    public void LimpiarLogrosPartida()
    {
        logrosObtenidosEnEstaPartida.Clear();
        mision1Superada = false;
        mision2Superada = false;
        mision3Superada = false;
        mision4Superada = false;
        mision5Superada = false;
    }
}