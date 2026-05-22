using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 

public class ControlMiniJuego : MonoBehaviour
{
    public float tiempo = 90f;

    void Start()
    {
        Invoke("VolverAlJuego", tiempo);
    }

    void VolverAlJuego()
    {
        
        int m = GestionPuntuacion.instancia.monedasDelRio;
        int p = GestionPuntuacion.instancia.puntosMinijuego; 
        int b = 15; 
        if (ApiBootstrapper.Instancia != null)
        {
            ApiBootstrapper.Instancia.GuardarPartidaFinalizada(m, p, b);
        }

        StartCoroutine(EsperarYCambiarEscena());
    }

    
    IEnumerator EsperarYCambiarEscena()
    {
        yield return new WaitForSeconds(1.0f);
        ControlCapibara.volverDeMiniJuego = true;
        SceneManager.LoadScene("Juego_prin");
    }
}