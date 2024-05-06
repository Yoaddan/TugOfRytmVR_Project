using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTutorialCanvas : MonoBehaviour
{
    public SongManager songManager; // Referencia a instancia del SongManager.

    // Método para cambiar al siguiente canvas o simplemente ocultar el actual
    public void HideCanvas()
    {
        gameObject.SetActive(false); // Oculta este canvas actual.
        songManager.StartSongAfterTutorial(); // Inicia todo tras acabar el tutorial.
    }

}

