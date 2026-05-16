using UnityEngine;
using TMPro;

public class ControlMensajes : MonoBehaviour
{
    public GameObject panelMensaje;
    public TextMeshProUGUI textoDelMensaje;

    public void MostrarMensaje(string mensajeNuevo)
    {
        panelMensaje.SetActive(true);
        textoDelMensaje.text = mensajeNuevo;

        CancelInvoke("OcultarMensaje");
        Invoke("OcultarMensaje", 8f);
    }

    void OcultarMensaje()
    {
        panelMensaje.SetActive(false);
    }
}