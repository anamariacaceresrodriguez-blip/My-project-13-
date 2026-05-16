using UnityEngine;
using TMPro; // para TextMeshPro

public class ContadorMonedas : MonoBehaviour
{
    public TextMeshProUGUI textoMonedas;
    private int cantidadMonedas = 0;

    void Start()
    {
        ActualizarInterfaz();
    }

    public void SumarMoneda(int valor)
    {
        cantidadMonedas += valor;
        ActualizarInterfaz();
    }

    void ActualizarInterfaz()
    {
        textoMonedas.text = cantidadMonedas.ToString();
    }
}