using UnityEngine;
using System.Collections.Generic;

public class RockManager : MonoBehaviour
{
    public GameObject rockPrefab; // Prefab de la roca
    public SongManager songManager; // Instancia del SongManager.
    public AudioSource rockDestroySFX; // Sonido de destrucción de la roca
    public Lane lane; // Referencia al Lane que contiene los timestamps
    public Transform spawnPoint; // Punto de spawn predefinido

    void Start()
    {
        if(!songManager.tutorialEnabled)
            Invoke("InitializeRockTimestamps", 0.1f); // Retrasa ligeramente la inicialización para asegurar que todos los sistemas están listos
    }

    void Update()
    {
        if(songManager.tutorialCompleted)
        {
            Invoke("InitializeRockTimestamps", 0.1f); // Retrasa ligeramente la inicialización para asegurar que todos los sistemas están listos
            songManager.tutorialCompleted = false;
        }
    }

    void InitializeRockTimestamps()
    {
        if (lane == null || lane.timeStamps.Count <= 2) {
            Debug.LogError("Lane no está listo o no tiene suficientes timestamps.");
            return;
        }
        
        // Seleccionar solo dos timestamps aleatorios
        float randomTimeStamp1 = (float)lane.timeStamps[Random.Range(0, lane.timeStamps.Count)];
        float randomTimeStamp2 = (float)lane.timeStamps[Random.Range(0, lane.timeStamps.Count)];
        
        ScheduleRockSpawn(randomTimeStamp1);
        ScheduleRockSpawn(randomTimeStamp2);
    }

    void ScheduleRockSpawn(float delay)
    {
        if (delay < 0) delay = 0.1f; // Asegura un mínimo delay para evitar tiempos negativos
        Invoke("SpawnRock", delay);
    }

    void SpawnRock()
    {
        if (spawnPoint != null) {
            Instantiate(rockPrefab, spawnPoint.position, spawnPoint.rotation);
        } else {
            Debug.LogError("No se ha definido un punto de spawn");
        }
    }

    public void PlayRockDestroySound()
    {
        rockDestroySFX.Play();
    }
}

