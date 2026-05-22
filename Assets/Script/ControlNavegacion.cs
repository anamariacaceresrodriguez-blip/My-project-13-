using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlNavegacion : MonoBehaviour
{
    public AudioSource fuenteMenu;
    public AudioClip sonidoClick;

    // 1. De Minijuego 2D a Escena Principal (Cuando ganas o terminas el minijuego)
    public void VolverAJuegoPrincipal()
    {
        Debug.Log(">>> Guardando datos al presionar Volver al Juego Principal...");

        // --- GUARDADO ANTES DE SALIR ---
        if (ApiBootstrapper.Instancia != null && GestionPuntuacion.instancia != null)
        {
            int monedas = GestionPuntuacion.instancia.monedasDelRio;
            int puntos = GestionPuntuacion.instancia.puntosMinijuego;

            // Si los puntos en GestionPuntuacion siguen en 0, intentamos rescatar los del GameManager local antes de irnos
            if (puntos == 0 && GameManager.instancia != null)
            {
                puntos = GameManager.instancia.puntosTotales;
            }

            ApiBootstrapper.Instancia.GuardarPartidaFinalizada(monedas, puntos, puntos);
        }

        ControlCapibara.volverDeMiniJuego = true;
        ControlCapibara.guardarPosicion = true;
        SceneManager.LoadScene("Juego_prin");
    }
    public void ReproducirSonidoBoton()
    {
        if (fuenteMenu != null && sonidoClick != null)
        {
            fuenteMenu.PlayOneShot(sonidoClick);
        }
    }

    // 2. De Juego Principal a Menú (Boton de salir al Menú)
    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        Debug.Log(">>> Guardando datos al presionar salir al Menú...");

        // --- GUARDADO ANTES DE IR AL MENÚ ---
        if (ApiBootstrapper.Instancia != null && GestionPuntuacion.instancia != null)
        {
            int monedas = GestionPuntuacion.instancia.monedasDelRio;
            int puntos = GestionPuntuacion.instancia.puntosMinijuego;

            if (puntos == 0 && GameManager.instancia != null)
            {
                puntos = GameManager.instancia.puntosTotales;
            }

            ApiBootstrapper.Instancia.GuardarPartidaFinalizada(monedas, puntos, 0);
        }

        ControlCapibara.volverDeMiniJuego = false;
        ControlCapibara.posicionGuardada = Vector3.zero;

        if (GestionPuntuacion.instancia != null)
        {
            GestionPuntuacion.instancia.ResetearTodo();
        }
        SceneManager.LoadScene("Menu");
    }

    public void ReiniciarNivelActual()
    {
        // 1. Esto despierta a Unity obligatoriamente antes de mover la escena
        Time.timeScale = 1f;

        Debug.Log(">>> Reiniciando nivel en ceros (No se guarda partida)...");

        // 2. Limpiamos la memoria interna de Unity
        if (GestionPuntuacion.instancia != null)
        {
            GestionPuntuacion.instancia.ResetearTodo();
        }

        // 3. Recargamos
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IrAlComandos() { SceneManager.LoadScene("comandos"); }
    public void IrAlCreditos() { SceneManager.LoadScene("creditos"); }
    public void IrAlLevelDesign() { SceneManager.LoadScene("Level Design"); }
    public void IrVideoInicial() { SceneManager.LoadScene("Video_inicial"); }

    // 3. Salir del Juego por completo
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