using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockManager : MonoBehaviour
{
    public GameObject rockPrefab; // Prefab de la roca
    public AudioSource rockDestroySFX; // Sonido de destrucción de la roca
    public Lane lane; // Referencia al Lane que contiene los timestamps
    public Transform spawnPoint; // Punto de spawn predefinido

    private List<float> selectedTimestamps = new List<float>(); // Timestamps seleccionados para spawnear rocas

    void Start()
    {
        StartCoroutine(WaitForTimestamps());
    }

    IEnumerator WaitForTimestamps()
    {
        // Espera hasta que los timestamps estén cargados
        yield return new WaitUntil(() => lane != null && lane.timeStamps.Count > 2);

        // Proceder con la selección de timestamps y spawnear rocas
        selectedTimestamps.Add((float)lane.timeStamps[Random.Range(0, lane.timeStamps.Count)]);
        selectedTimestamps.Add((float)lane.timeStamps[Random.Range(0, lane.timeStamps.Count)]);
        StartCoroutine(SpawnRocksAtTimestamps());
    }

    IEnumerator SpawnRocksAtTimestamps()
    {
        foreach (var timestamp in selectedTimestamps)
        {
            float waitTime = timestamp - Time.time;
            yield return new WaitForSeconds(waitTime > 0 ? waitTime : 0);
            SpawnRock();
        }
    }

    void SpawnRock()
    {
        if (spawnPoint != null)
        {
            GameObject rock = Instantiate(rockPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("No se ha definido un punto de spawn");
        }
    }

    public void PlayRockDestroySound()
    {
        rockDestroySFX.Play();
    }
}
