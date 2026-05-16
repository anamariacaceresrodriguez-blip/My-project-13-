using UnityEngine;

public class Musica_fondo : MonoBehaviour
{
    private void Awake()
    {
        
        DontDestroyOnLoad(this.gameObject);

       
        if (FindObjectsOfType<Musica_fondo>().Length > 1)
        {
            Destroy(gameObject);
        }
    }
}