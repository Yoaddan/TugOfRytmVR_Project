using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class HealthBar : MonoBehaviour
{
    public Slider slider;  // Referencia al Slider UI

    void Start()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
    }

    // Método público para establecer la salud
    public void SetHealth(int health)
    {
        slider.value = health;
    }

    // Método para decrementar la salud
    public void TakeDamage(int damage)
    {
        slider.value -= damage;
        if (slider.value < 0)
            slider.value = 0;
    }

    // Verificar si la salud es 0 y actuar en consecuencia
    void Update()
    {
        if (slider.value <= 0)
        {
            Die();
        }
    }

    // Manejar la "muerte" del jugador
    private void Die()
    {
        // Cargar la escena con índice 0
        SceneManager.LoadScene(0);
    }

}
