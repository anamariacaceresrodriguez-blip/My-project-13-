using UnityEngine;

public class Basura_rio : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ControlCapibara capibara = other.GetComponent<ControlCapibara>();

            if (capibara != null)
            {
                // Le pasa esta misma basura como parámetro
                capibara.RecogerBasura(gameObject);
            }
            else
            {
                Debug.Log("No se encontró el script ControlCapibara en el objeto Player");
            }
        }
    }
}