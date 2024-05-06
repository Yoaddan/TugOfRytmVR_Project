using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvas: MonoBehaviour
{
    public GameObject nextCanvas; // El siguiente canvas a mostrar (opcional)

    // Método para cambiar al siguiente canvas o simplemente ocultar el actual
    public void ShowNextCanvas()
    {
        if (nextCanvas != null) {
            nextCanvas.SetActive(true); // Activa el siguiente canvas si está asignado
        }
        
        gameObject.SetActive(false); // Oculta este canvas actual
    }
}
