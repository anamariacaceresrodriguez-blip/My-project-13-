using System;
using System.Collections;
using System.Net.NetworkInformation;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


public static class GameSession
{
    public static int IdUsuarioActual = 42;
    public static int IdPartidaActual = 1;
    public static string NombreEquipoActual = "";
}

[System.Serializable]
public class UsuarioDto { public int IdUsuario; public string NombreUsuario; public string Correo; public int Edad; }

[System.Serializable]

public class PartidaDto
{
    public int IdPartida;
    public int IdUsuario;
    public string Escenario;
    public int MonedasRecogidas;
    public int Puntos; 
}

[System.Serializable]
public class HistorialReciclajeDto { public int IdRegistro; public int IdPartida; public string TipoResiduo; public int Cantidad; }

[System.Serializable]
public class LogroDto
{
    public int IdLogro;
    public string NombreLogro; 
    public int IdPartida;
}

public class ApiBootstrapper : MonoBehaviour
{
    public static ApiBootstrapper Instancia { get; private set; }

    [Header("Configuración de Red")]
    public string puertoLocal = "7186";

    private string urlUsuarios;
    private string urlPartidas;
    private string urlReciclaje;
    private string urlLogros;

    [HideInInspector]
    public System.Collections.Generic.List<string> basuraAcumuladaEnRio = new System.Collections.Generic.List<string>();

    void Awake()
    {
        if (Instancia == null) { Instancia = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        urlUsuarios = $"https://localhost:{puertoLocal}/api/Usuarios";
        urlPartidas = $"https://localhost:{puertoLocal}/api/Partidas";
        urlReciclaje = $"https://localhost:{puertoLocal}/api/historial_reciclaje";
        urlLogros = $"https://localhost:{puertoLocal}/api/Logros";

        string nombreComputador = Environment.UserName;
        GameSession.NombreEquipoActual = nombreComputador;

        StartCoroutine(ConectarORegistrarUsuario(nombreComputador));
    }

    private IEnumerator ConectarORegistrarUsuario(string nombrePC)
    {
        UsuarioDto dtoUsuario = new UsuarioDto { IdUsuario = 0, NombreUsuario = nombrePC, Correo = nombrePC.ToLower() + "@ecoaventura.local", Edad = 21 };
        string json = JsonUtility.ToJson(dtoUsuario);

        using (UnityWebRequest request = new UnityWebRequest(urlUsuarios, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success || request.responseCode == 201)
            {
                try
                {
                    UsuarioDto usuarioRegistrado = JsonUtility.FromJson<UsuarioDto>(request.downloadHandler.text);
                    if (usuarioRegistrado != null && usuarioRegistrado.IdUsuario > 0) GameSession.IdUsuarioActual = usuarioRegistrado.IdUsuario;
                }
                catch { }
            }
            if (GameSession.IdUsuarioActual == 0) GameSession.IdUsuarioActual = 1;
        }
    }

    public void GuardarPartidaFinalizada(int monedas, int puntos, int basura)
    {
        if (GameSession.IdUsuarioActual == 0)
        {
            Debug.LogError("Error: Aún no se ha identificado al usuario.");
            return;
        }

        PartidaDto p = new PartidaDto
        {
            IdPartida = 0,
            IdUsuario = GameSession.IdUsuarioActual,
            Escenario = "Amazonas",
            MonedasRecogidas = monedas, 
            Puntos = puntos             
        };

        StartCoroutine(EnviarPartidaFinal(p));
    }

    private IEnumerator EnviarPartidaFinal(PartidaDto p)
    {
        string json = JsonUtility.ToJson(p);
        Debug.Log(">>> JSON ENVIADO: " + json);

        string urlFinal = $"https://localhost:{puertoLocal}/api/Partidas";

        using (UnityWebRequest req = UnityWebRequest.PostWwwForm(urlFinal, json))
        {
            req.certificateHandler = new BypassCertificate(); 

            byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(body);
            req.downloadHandler = new DownloadHandlerBuffer();

            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(">>> ¡ÉXITO TOTAL DE PERSISTENCIA!: Partida guardada en SQL.");
                Debug.Log(">>> RESPUESTA SERVIDOR: " + req.downloadHandler.text);

                try
                {
                  
                    PartidaDto partidaCreada = JsonUtility.FromJson<PartidaDto>(req.downloadHandler.text);
                    int idRealAutogenerado = partidaCreada.IdPartida;

                    
                    GameSession.IdPartidaActual = idRealAutogenerado;

                    Debug.Log(">>> [API] IdPartida asignado con éxito por SQL: " + idRealAutogenerado);

                 
                    Debug.Log($">>> Despachando {basuraAcumuladaEnRio.Count} residuos al historial con ID: {idRealAutogenerado}...");
                    foreach (string residuo in basuraAcumuladaEnRio)
                    {
                        StartCoroutine(EnviarReciclaje(residuo, 1, idRealAutogenerado));
                    }
                    basuraAcumuladaEnRio.Clear();

                    
                    if (LogrosManager.Instancia != null)
                    {
                        int totalLogros = LogrosManager.Instancia.logrosObtenidosEnEstaPartida.Count;
                        Debug.Log($">>> Despachando {totalLogros} misiones superadas a SQL con ID: {idRealAutogenerado}...");

                        foreach (string logroNombre in LogrosManager.Instancia.logrosObtenidosEnEstaPartida)
                        {
                            StartCoroutine(EnviarLogro(logroNombre, "Misión superada en juego", idRealAutogenerado));
                        }
                        LogrosManager.Instancia.LimpiarLogrosPartida();
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(">>> Error en la automatización relacional: " + ex.Message);
                }
            }
            else
            {
                Debug.LogError(">>> CÓDIGO DE RESPUESTA: " + req.responseCode);
                Debug.LogError(">>> DETALLE TÉCNICO: " + req.error);
                Debug.LogError(">>> QUÉ RECHAZÓ EL SERVIDOR: " + req.downloadHandler.text);
            }
        }
    }
    
    private IEnumerator EnviarReciclaje(string tipo, int cant, int idPartidaReal)
    {
        HistorialReciclajeDto dto = new HistorialReciclajeDto
        {
            IdRegistro = 0,
            IdPartida = idPartidaReal, 
            TipoResiduo = tipo,
            Cantidad = cant
        };

        string json = JsonUtility.ToJson(dto);
        string urlFinal = $"https://localhost:{puertoLocal}/api/historial_reciclaje";

        using (UnityWebRequest req = UnityWebRequest.PostWwwForm(urlFinal, json))
        {
            req.certificateHandler = new BypassCertificate();

            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($">>> [SQL] Historial guardado con éxito: {tipo} amarrado a la Partida {idPartidaReal}");
            }
            else
            {
                Debug.LogError($">>> Error en Historial para {tipo}: {req.error} | {req.downloadHandler.text}");
            }
        }
    }


    public void RegistrarLogroObtenido(string nombre, string descripcion, int idPartidaReal)
    {
        StartCoroutine(EnviarLogro( nombre, descripcion, idPartidaReal));
    }

    
    private IEnumerator EnviarLogro(string nombre, string descrpcion, int idPartidaReal)
    {
        LogroDto dto = new LogroDto
        {
            IdLogro = 0,
            IdPartida = idPartidaReal, 
            NombreLogro = nombre
        };

        string json = JsonUtility.ToJson(dto);
        string urlFinal = $"https://localhost:{puertoLocal}/api/Logros";

        using (UnityWebRequest req = UnityWebRequest.PostWwwForm(urlFinal, json))
        {
            req.certificateHandler = new BypassCertificate();
            byte[] body = Encoding.UTF8.GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(body);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($">>> ¡LOGRO GUARDADO EN SQL RELACIONAL!: {nombre} en Partida {idPartidaReal}");
            }
            else
            {
                Debug.LogError($">>> Error al guardar el logro {nombre}: {req.error}");
            }
        }
    }





    public class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}