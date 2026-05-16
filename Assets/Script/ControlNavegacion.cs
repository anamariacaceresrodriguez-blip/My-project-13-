using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlNavegacion : MonoBehaviour
{




    // 1. De Minijuego 2D a Escena Principal
    public void VolverAJuegoPrincipal()
    {
        ControlCapibara.volverDeMiniJuego = true;
        ControlCapibara.guardarPosicion = true;
        SceneManager.LoadScene("Juego_prin");
    }

    public AudioSource fuenteMenu;
    public AudioClip sonidoClick;

    public void ReproducirSonidoBoton()
    {
        if (fuenteMenu != null && sonidoClick != null)
        {
            fuenteMenu.PlayOneShot(sonidoClick);
        }
    }

    // 2. De Juego Principal a Menú
    public void IrAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void IrAlComandos()
    {
        SceneManager.LoadScene("comandos");
    }

    public void IrAlCreditos()
    {
        SceneManager.LoadScene("creditos");
    }

    public void IrAlLevelDesign()
    {
        SceneManager.LoadScene("Level Design");
    }

    public void IrVideoInicial()
    {
        SceneManager.LoadScene("Video_inicial");
    }


    public void ReiniciarPartida()
    {
        Time.timeScale = 1f;

        ControlCapibara.volverDeMiniJuego = false;
        ControlCapibara.posicionGuardada = Vector3.zero;

        if (GestionPuntuacion.instancia != null)
        {
            GestionPuntuacion.instancia.ResetearTodo();
        }

        SceneManager.LoadScene("Level Design");
    }

    // 3. Salir del Juego
    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    public class CambioEscenas : MonoBehaviour
    {
        public void EmpezarJuego()
        {
            SceneManager.LoadScene("Juego_prin");
        }
    }
}