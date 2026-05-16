using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlMiniJuego : MonoBehaviour
{
    public float tiempo = 90f;

    void Start()
    {
        Invoke("VolverAlJuego", tiempo);
    }

    void VolverAlJuego()
    {
        ControlCapibara.volverDeMiniJuego = true;

        SceneManager.LoadScene("Juego_prin"); // CAMBIA ESTE NOMBRE
    }
}