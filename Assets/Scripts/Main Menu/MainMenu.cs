using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject tutorialPopup; // Referencia al Canvas del pop-up de tutorial

    void Start()
    {
        if (tutorialPopup != null)
            tutorialPopup.SetActive(false); // Asegurarse de que el pop-up esté oculto al inicio
    }

    // Método para ir a la demo (escena con índice 2)
    public void GoToDemo()
    {
        SceneManager.LoadScene(2);
    }

    // Método para ir al tutorial (escena con índice 1)
    public void GoToTutorial()
    {
        SceneManager.LoadScene(1);
    }

    // Método para cerrar el juego
    public void ExitGame()
    {
        Application.Quit();
    }

    // Método para mostrar el pop-up de tutorial
    public void PopUpTutorial()
    {
        if (tutorialPopup != null)
            tutorialPopup.SetActive(true);
    }
}
