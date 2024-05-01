using System;
using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;  // Restricción de la nota MIDI específica para este carril
    public KeyCode input;  // Tecla de entrada para interactuar con las notas
    public GameObject notePrefab;  // Prefab de la nota que se instanciará
    List<Note> notes = new List<Note>();  // Lista para mantener un seguimiento de las notas instanciadas
    public List<double> timeStamps = new List<double>();  // Momentos específicos para instanciar notas

    int spawnIndex = 0;  // Índice para el control de la instanciación
    int inputIndex = 0;  // Índice para el control de interacción de usuario con las notas

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }

    void Update()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
            {
                GameObject note = Instantiate(notePrefab, transform.position, Quaternion.identity, transform);
                Note noteComponent = note.GetComponent<Note>();
                notes.Add(noteComponent);
                noteComponent.assignedTime = (float)timeStamps[spawnIndex];
                spawnIndex++;
            }
        }

        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (Input.GetKeyDown(input))
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    Hit();
                    Debug.Log($"Hit on {inputIndex} note");
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;
                }
                else
                {
                    Debug.Log($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
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

    private void Hit()
    {
        ScoreManager.Hit();
    }

    private void Miss()
    {
        ScoreManager.Miss();
    }
}
