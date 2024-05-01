using UnityEngine;
using UnityEngine.UI;

public class RhythmIndicator : MonoBehaviour
{
    Image rhythmCircle;
    float initialScale = 1.0f;  // La escala inicial del círculo
    float targetTime;           // Tiempo en el que el círculo debe desaparecer completamente

    void Start()
    {
        rhythmCircle = GetComponentInChildren<Image>();
        // Realiza un casting explícito aquí si targetTime es un double
        targetTime = (float)transform.parent.GetComponent<Note>().assignedTime;  
    }

    void Update()
    {
        float currentTime = (float)SongManager.GetAudioSourceTime(); // Asegúrate de castear a float si es necesario
        float timeLeft = targetTime - currentTime;

        if (timeLeft > 0)
        {
            float scale = timeLeft / SongManager.Instance.noteTime;  // Ajustar este valor según necesidades
            rhythmCircle.transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            rhythmCircle.gameObject.SetActive(false);  // Ocultar el indicador cuando el tiempo ha pasado
        }
    }
}
