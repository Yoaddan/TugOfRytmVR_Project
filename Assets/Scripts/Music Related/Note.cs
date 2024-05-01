using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime; // Tiempo asignado para cuando la nota debe ser tocada
    public RectTransform rhythmIndicator; // UI que indica cuándo tocar la nota
    private float progress = 0f; // Progreso de la nota desde el inicio al final de la cuerda
    public float speed = 1f; // Velocidad a la que la nota se mueve a lo largo de la cuerda
    public Transform[] pathPoints; // Huesos o puntos del camino a seguir
    public float yOffset = 0.025f; // Desplazamiento vertical para que las notas estén un poco arriba de la cuerda

    void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
        if (rhythmIndicator != null)
        {
            rhythmIndicator.localScale = Vector3.one;
        }
    }

    void Update()
    {
        UpdateNotePosition();
    }

    void UpdateNotePosition()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2)); // Calcular el progreso hacia el tiempo asignado

        if (t > 1)
        {
            Destroy(gameObject); // Destruye la nota si su tiempo ha expirado
        }
        else
        {
            progress = Mathf.Clamp01(t * speed);
            if (pathPoints != null && pathPoints.Length > 1)
            {
                FollowPath();
            }

            if (rhythmIndicator != null)
            {
                rhythmIndicator.localScale = Vector3.one * (1 - t);
            }
        }
    }

    void FollowPath()
    {
        if (pathPoints == null || pathPoints.Length < 2) return;
        
        int segment = Mathf.Min((int)(progress * (pathPoints.Length - 1)), pathPoints.Length - 2);
        float segmentProgress = (progress * (pathPoints.Length - 1)) - segment;
        Vector3 interpolatedPosition = Vector3.Lerp(pathPoints[segment].position, pathPoints[segment + 1].position, segmentProgress);
        transform.position = new Vector3(interpolatedPosition.x, interpolatedPosition.y + yOffset, interpolatedPosition.z);
    }
}
