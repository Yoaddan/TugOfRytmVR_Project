using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement; 

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public Lane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError; // En segundos.
    private bool songHasStarted = false; // Marca que la canción realmente ha comenzado.
    public GameObject endGameCanvas;  // Referencia al Canvas que contiene el UI del fin del juego.
    public GameObject tutorialCanvas; // Canvas del tutorial.
    public bool tutorialEnabled = false; // Controla si el tutorial está habilitado.
    public bool tutorialCompleted = false; // Variable referencia para otras instancias.

    public int inputDelayInMilliseconds;
    

    public string fileLocation;
    public float noteTime;

    public static MidiFile midiFile;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Note.ResetMaterialIndex();  // Resetear el índice antes de iniciar cualquier cosa relacionada con las notas
        StartCoroutine(ReadFromWebsite());
        if (tutorialEnabled) {
            tutorialCanvas.SetActive(true); // Muestra el tutorial si está habilitado
        } 
    }

    private IEnumerator ReadFromWebsite()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLocation))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();
                }
            }
        }
    }

    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var lane in lanes) lane.SetTimeStamps(array);

        if (!tutorialEnabled) // La empieza inmediatamente si esta desactivado el tutorial.
        {
            StartSongAfterTutorial();
        } 

    }

    public void StartSongAfterTutorial()
    {
        tutorialCompleted = true;
        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    public void StartSong()
    {
        audioSource.Play();
        songHasStarted = true; // Marca que la canción realmente ha comenzado
    }

    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

    void Update()
    {
        if (songHasStarted && !audioSource.isPlaying)
        {
            ShowEndGameCanvas(); // Mostrar el canvas en lugar de cargar la escena
        }
    }

    void ShowEndGameCanvas()
    {
        endGameCanvas.SetActive(true); // Activar el canvas del fin del juego
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0); // Método llamado por el botón para cargar la escena principal
    }

}