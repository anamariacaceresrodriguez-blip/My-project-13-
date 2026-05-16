using UnityEngine;

public class MovimientoAnimal : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float rapidez = 2f;
    public float distanciaDeteccion = 1.0f;
    public LayerMask capaMontaña; 

    private int direccion = 1; 

    void Update()
    {
        
        Vector3 movimiento = new Vector3(direccion, 0, 0);

        //  Mover al animal
        transform.Translate(movimiento * rapidez * Time.deltaTime);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, movimiento, out hit, distanciaDeteccion, capaMontaña))
        {
     
            direccion *= -1;

            
            transform.Rotate(0, 180, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 dir = new Vector3(direccion, 0, 0);
        Gizmos.DrawLine(transform.position, transform.position + dir * distanciaDeteccion);
    }
}