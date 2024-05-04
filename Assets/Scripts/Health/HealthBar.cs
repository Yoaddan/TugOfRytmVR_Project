using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

}
