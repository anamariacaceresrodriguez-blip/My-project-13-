using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    public static AudioManager instancia;

    public AudioSource fuenteEfectos;

    void Awake()
    {
        
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReproducirEfecto(AudioClip clip)
    {
        if (fuenteEfectos != null && clip != null)
        {
            fuenteEfectos.PlayOneShot(clip);
        }
    }
}