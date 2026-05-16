using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TemporizadorMinijuego : MonoBehaviour
{
    public float tiempoRestante = 60f;
    public TextMeshProUGUI textoTiempo;

    void Update()
    {
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            ActualizarUI();
        }
        else
        {
            FinalizarMinijuego();
        }
    }

    void ActualizarUI()
    {
        // Formatea el tiempo 
        int segundos = Mathf.CeilToInt(tiempoRestante);
        textoTiempo.text = "Tiempo: " + segundos + "s";
    }

    void FinalizarMinijuego()
    {
        
        SceneManager.LoadScene("Level Design");
    }


}