using System;
using Melanchall.DryWetMidi.Interaction;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public static Lane Instance;  // Singleton para accesibilidad global dentro de la escena.
    public Transform[] pathPoints;  // Puntos que definen la ruta de la cuerda.
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;  // Filtro para las notas MIDI específicas de este carril.
    public KeyCode input;  // Tecla para interacción.
    public GameObject notePrefab;  // Prefab de la nota que se instanciará.
    List<Note> notes = new List<Note>();  // Lista para mantener un seguimiento de las notas instanciadas.
    public List<double> timeStamps = new List<double>();  // Momentos específicos para instanciar notas.
    
    int spawnIndex = 0;  // Índice para el control de la instanciación.
    int inputIndex = 0;  // Índice para el control de interacción de usuario con las notas.

    void Awake()
    {
        Instance = this;  // Configura la instancia única.
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60 + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000);
            }
        }
    }

    void Update()
    {
        if (spawnIndex < timeStamps.Count && SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
        {
            Debug.Log("Spawneo al elemento: "+ spawnIndex);
            SpawnNote();
            spawnIndex++;
        }

        if (inputIndex < timeStamps.Count && inputIndex < notes.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if ( notes[inputIndex].collision || Input.GetKeyDown(input) ) // Verifica su colision y si se hizo en el tiempo adecuado.
            {
                if(Math.Abs(audioTime - timeStamp) < marginOfError) // Verifica su colision y si se hizo en el tiempo adecuado.
                {
                    Hit();
                    Debug.Log($"Hit on {inputIndex} note");
                    if (notes[inputIndex])  // Esta línea verifica si la nota aún existe y no fue destruida en el momento exacto, para cuando el hit es extremadamente preciso.
                    {
                        Destroy(notes[inputIndex].gameObject);
                    }
                    inputIndex++;
                } 
                else 
                {
                    Miss();
                    Debug.Log($"Missed {inputIndex} note, but tried to hit.");
                    if (notes[inputIndex])  // Esta línea verifica si la nota aún existe y no fue destruida en el momento exacto, para cuando el hit es extremadamente preciso.
                    {
                        Destroy(notes[inputIndex].gameObject);
                    }
                    inputIndex++;
                }
            }
            if (audioTime >= timeStamp + marginOfError)
            {
                Miss();
                Debug.Log($"Missed {inputIndex} note");
                inputIndex++;
            }
        }
    }

    void SpawnNote()
    {
        if (pathPoints.Length == 0) return;  // Asegurarse de que haya puntos en la ruta.
        GameObject noteObject = Instantiate(notePrefab, pathPoints[0].position, Quaternion.identity, transform);
        Note noteComponent = noteObject.GetComponent<Note>();
        notes.Add(noteComponent);
        noteComponent.assignedTime = (float)timeStamps[spawnIndex];
        noteComponent.pathPoints = pathPoints;  // Proporciona los puntos de la trayectoria a la nota
    }

    private void Hit()
    {
        ScoreManager.Hit();  // Lógica de acierto.
    }

    private void Miss()
    {
        ScoreManager.Miss();  // Lógica de fallo.
    }
}
