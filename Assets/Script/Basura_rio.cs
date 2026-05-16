using UnityEngine;

public class Basura_rio : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var capibara = other.GetComponent<ControlCapibara>();

            if (capibara != null)
            {
                capibara.RecogerBasura();
            }
            else
            {
                Debug.Log("No se encontró el script ControlCapibara");
            }

            Destroy(gameObject);
        }
    }
}