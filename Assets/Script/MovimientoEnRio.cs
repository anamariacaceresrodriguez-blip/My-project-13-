using UnityEngine;

public class MovimientoEnRio : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 5f;
  
    public float amplitud = 110f;

    private Vector3 posicionInicial;

    void Start()
    {
        
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Calculamos  eje X
        float x = Mathf.PingPong(Time.time * velocidad, amplitud * 2) - amplitud;

        // Aplicala nueva posición manteniendo Y y Z 
        transform.position = new Vector3(posicionInicial.x + x, posicionInicial.y, posicionInicial.z);

        // Rotación automática para que miren hacia donde caminan
        if (velocidad > 0)
        {
            if (x > (amplitud - 0.5f)) 
            {
                
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }
            else if (x < (-amplitud + 0.5f)) 
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
    }
}