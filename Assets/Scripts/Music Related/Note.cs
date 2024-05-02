using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    public Material[] materials; // Arreglo de materiales asignados desde el inspector
    private static int nextMaterialIndex = 0; // Índice estático para seguir el próximo material a usar
    double timeInstantiated;
    public float assignedTime; // Tiempo en el que la nota debe estar en su posición final
    public RectTransform rhythmIndicator; // Indicador visual de ritmo
    public Transform[] pathPoints; // Puntos de la trayectoria de la nota
    public float yOffset = 0.025f; // Desplazamiento vertical para mantener la nota sobre la cuerda
    public float earlyArrivalFactor = 0.9f; // La nota llegará al final del recorrido al 90% del tiempo asignado

    void Start()
    {
        GetComponent<Renderer>().material = materials[nextMaterialIndex]; // Obtiene su renderer
        nextMaterialIndex = (nextMaterialIndex + 1) % materials.Length; // Asegura que el índice siempre sea válido
        timeInstantiated = SongManager.GetAudioSourceTime(); // Captura el tiempo cuando la nota es instanciada
        if (rhythmIndicator != null)
        {
            rhythmIndicator.localScale = Vector3.one; // Inicia el indicador a escala completa
        }
    }

    void Update()
    {
        UpdateNotePosition();
    }

    void UpdateNotePosition()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / SongManager.Instance.noteTime); // Calcula el progreso basado en el tiempo asignado
        t = Mathf.Clamp01(t); // Asegura que t se mantenga entre 0 y 1

        if (t >= 1f)
        {
            Destroy(gameObject); // Destruye la nota si su tiempo ha expirado
        }
        else
        {
            FollowPath(t, earlyArrivalFactor);
            if (rhythmIndicator != null)
            {
                rhythmIndicator.localScale = Vector3.one * Mathf.Lerp(1f, 0.27f, t);
            }
        }
    }

    void FollowPath(float t, float earlyArrivalFactor)
    {
        if (pathPoints == null || pathPoints.Length < 2) return;

        // Ajuste de 't' para que la nota llegue más rápido al final
        float adjustedT = Mathf.Clamp01(t / earlyArrivalFactor);

        int pathIndex = Mathf.FloorToInt(adjustedT * (pathPoints.Length - 1));
        float localT = (adjustedT * (pathPoints.Length - 1)) - pathIndex;
        Vector3 startPosition = pathPoints[pathIndex].position;
        Vector3 endPosition = pathPoints[Mathf.Min(pathIndex + 1, pathPoints.Length - 1)].position;

        transform.position = Vector3.Lerp(startPosition, endPosition, localT) + new Vector3(0, yOffset, 0);
    }
}

