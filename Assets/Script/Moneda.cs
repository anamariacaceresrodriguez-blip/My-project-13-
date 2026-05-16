using UnityEngine;

public class Moneda : MonoBehaviour
{
    [Header("Sonido de Recolección")]
    public AudioClip sonidoMoneda;
    [Range(0f, 1f)] public float volumen = 1f; // Nueva variable para controlar el volumen

    [Header("Lista de Mensajes Ambientales y Reciclaje")]
    public string[] mensajesPosibles = {
        "¡Recuerda reciclar para proteger el río!",
        "No arrojes basura al agua, los animales te lo agradecen.",
        "El agua limpia es vida para el capibara.",
        "Reduce el uso de plásticos de un solo uso.",
        "Cada objeto reciclado cuenta para el medio ambiente.",
        "Caneca Verde: \"Deposita aquí las cáscaras de banano, ya que son residuos orgánicos que pueden volver a la tierra como abono.\"",
        "Caneca Negra: \"En la caneca negra va el pañal, ya que es un residuo que no tiene más oportunidad de ser aprovechado y debe ir a disposición final.\"",
        "Caneca Blanca: \"Las botellas y latas van en la caneca blanca, porque son materiales que pueden transformarse y tener una nueva vida.\"",
        "Caneca Verde: \"Todo lo que sea resto de comida o poda va en la verde, porque su naturaleza orgánica permite que se degrade naturalmente.\"",
        "Caneca Negra: \"Las servilletas sucias y el papel higiénico van en la negra, pues al estar contaminados ya no sirven para reciclar.\"",
        "Caneca Blanca: \"El papel y el cartón limpio van en la blanca, ya que reciclarlos evita que se corten más árboles.\"",
        "Aviso Especial: \"Recuerda que las llantas no van en ninguna caneca, ya que por su tamaño y material requieren un proceso especial para no dañar el ecosistema.\"",
        "Caneca Negra: \"Lo que esté muy engrasado o sucio va en la negra, porque su estado impide que sea procesado nuevamente.\""
    };

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (sonidoMoneda != null)
            {
                // Ahora incluimos el volumen 
                AudioSource.PlayClipAtPoint(sonidoMoneda, transform.position, volumen);
            }

            ControlMensajes control = Object.FindFirstObjectByType<ControlMensajes>();

            if (control != null)
            {
                int indice = Random.Range(0, mensajesPosibles.Length);
                string mensajeAleatorio = mensajesPosibles[indice];
                control.MostrarMensaje(mensajeAleatorio);
            }

            Destroy(gameObject);
        }
    }
}