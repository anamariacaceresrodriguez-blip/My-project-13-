using UnityEngine;
using System.Collections;

public class ClimaPostMinijuego : MonoBehaviour
{
    [Header("Referencias de Efectos")]
    public GameObject objetoLluvia;
    public GameObject objetoNeblina;

    private AudioSource audioLluvia; 

    [Header("Tiempos de la Tormenta")]
    public float duracionTormenta = 8f;
    public float tiempoDespejado = 12f;

    void Start()
    {
        // Intentamos obtener el audio del objeto lluvia
        if (objetoLluvia != null)
            audioLluvia = objetoLluvia.GetComponent<AudioSource>();

        if (objetoLluvia != null) objetoLluvia.SetActive(false);
        if (objetoNeblina != null) objetoNeblina.SetActive(false);

        if (ControlCapibara.volverDeMiniJuego)
        {
            StartCoroutine(CicloDeTormenta());
        }
    }

    IEnumerator CicloDeTormenta()
    {
        while (true)
        {
            // ACTIVAR
            if (objetoLluvia != null) objetoLluvia.SetActive(true);
            if (objetoNeblina != null) objetoNeblina.SetActive(true);

            // Si hay audio
            if (audioLluvia != null) audioLluvia.Play();

            yield return new WaitForSeconds(duracionTormenta);

            // DESACTIVAR
            if (objetoLluvia != null) objetoLluvia.SetActive(false);
            if (objetoNeblina != null) objetoNeblina.SetActive(false);

            // Si hay audio
            if (audioLluvia != null) audioLluvia.Stop();

            yield return new WaitForSeconds(tiempoDespejado);
        }
    }
}