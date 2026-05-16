using UnityEngine;

public class MovimientoCanoa : MonoBehaviour
{
    [Header("Ajustes de Velocidad")]
    public float velocidadAvance = 10f;
    public float distanciaCarril = 3f;
    public float velocidadCambioCarril = 10f;

    private int carrilActual = 1;
    private bool estaPausado = false;

    void Update()
    {
        // Pausa
        if (Input.GetKeyDown(KeyCode.Space))
        {
            estaPausado = !estaPausado;
            Time.timeScale = estaPausado ? 0 : 1;
        }

        if (estaPausado) return;

     
        transform.Translate(Vector3.forward * velocidadAvance * Time.deltaTime);

       
        if (Input.GetKeyDown(KeyCode.A) && carrilActual > 0)
            carrilActual--;

        if (Input.GetKeyDown(KeyCode.D) && carrilActual < 2)
            carrilActual++;

        // Movimiento lateral
        float posicionObjetivoX = (carrilActual - 1) * distanciaCarril;

        Vector3 nuevaPosicion = transform.position;
        nuevaPosicion.x = Mathf.Lerp(transform.position.x, posicionObjetivoX, Time.deltaTime * velocidadCambioCarril);

        transform.position = nuevaPosicion;
    }
}