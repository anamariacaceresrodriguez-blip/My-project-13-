using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ReporteReciclaje : MonoBehaviour
{
    private string urlHistorial = "https://localhost:7123/api/Historial_reciclaje";

    // Llama a esta función desde el script donde detectas que el Capibara recoge basura
    public void EnviarResiduoBD(string tipoResiduo, int cantidadRecogida)
    {
        // Si por alguna razón la partida no se ha creado en el arranque, no enviamos nada
        if (GameSession.IdPartidaActual == 0)
        {
            Debug.LogWarning("No se puede guardar el historial porque no hay una partida activa en la BD.");
            return;
        }

        HistorialTemporal dtoHistorial = new HistorialTemporal
        {
            idPartida = GameSession.IdPartidaActual, // <--- CONEXIÓN MÁGICA: Se amarra a la partida del equipo
            tipoResiduo = tipoResiduo,               // "Plástico", "Vidrio" o "Lata"
            cantidad = cantidadRecogida
        };

        StartCoroutine(EnviarHistorialPost(dtoHistorial));
    }

    private IEnumerator EnviarHistorialPost(HistorialTemporal datos)
    {
        string json = JsonUtility.ToJson(datos);

        using (UnityWebRequest request = new UnityWebRequest(urlHistorial, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("¡Se insertó correctamente el residuo en la tabla Historial_reciclaje!");
            }
            else
            {
                Debug.LogError("Error al enviar el historial de reciclaje: " + request.error);
            }
        }
    }

    [Serializable]
    private class HistorialTemporal
    {
        public int idPartida;
        public string tipoResiduo;
        public int cantidad;
    }
}
