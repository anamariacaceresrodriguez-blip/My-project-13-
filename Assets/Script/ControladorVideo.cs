using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ControladorVideo : MonoBehaviour
{
    private VideoPlayer miVideoPlayer;

    void Start()
    {
        miVideoPlayer = GetComponent<VideoPlayer>();

  
        miVideoPlayer.loopPointReached += AlTerminarVideo;
    }

    void AlTerminarVideo(VideoPlayer vp)
    {
        
        SceneManager.LoadScene("Level Design");
    }

    void Update()
    {
        // saltar el video con cualquier tecla o clic
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("Level Design");
        }
    }
}