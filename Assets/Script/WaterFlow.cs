using UnityEngine;

public class WaterFlow : MonoBehaviour
{
    // Velocidad del flujo del agua
    public float speedY = 0.5f;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // Calcula el desplazamiento basado en el tiempo
        float offset = Time.time * speedY;

        // Aplicamos el movimiento al eje Y 
        
        rend.material.SetTextureOffset("_BaseMap", new Vector2(0, offset));
    }
}