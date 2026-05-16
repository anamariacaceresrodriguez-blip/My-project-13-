using UnityEngine;

public class CanecaClasificadora : MonoBehaviour
{
    [Header("Referencias")]
    public GameManager gameManager;
    public AudioSource fuenteAudio;
    public AudioClip sonidoPunto;
    public AudioClip sonidoError;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Tag del objeto que entra con el de esta caneca
        if (other.CompareTag(gameObject.tag))
        {
            Acierto();
        }
        else
        {
            // error para poder usar su tag
            Error(other);
        }

        // Destruir la basura 
        Destroy(other.gameObject);
    }

    void Acierto()
    {
        if (gameManager != null) gameManager.Sumar(); 

        if (fuenteAudio != null && sonidoPunto != null)
        {
            fuenteAudio.PlayOneShot(sonidoPunto);
        }
        Debug.Log("¡Correcto! Coincide con el tag: " + gameObject.tag);
    }

    // Collider2D
    void Error(Collider2D objetoBasura)
    {
      

        if (fuenteAudio != null && sonidoError != null)
        {
            fuenteAudio.PlayOneShot(sonidoError);
        }

        Debug.Log("Error: " + objetoBasura.tag + " no coincide con la caneca " + gameObject.tag);
    }
}