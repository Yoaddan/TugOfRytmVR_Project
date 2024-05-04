using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCollider : MonoBehaviour
{
    public AudioSource movedSFX;
    public Transform objectToRotate; // Objeto que será rotado
    public float rotationDegrees = 45.0f; // Grados de rotación
    public float cooldownTime = 5.0f; // Tiempo de espera en segundos entre rotaciones

    private float lastRotationTime = 0; // Cuando se realizó la última rotación

    void OnCollisionEnter(Collision collision)
    {
        // Verificar si el objeto con el que se colisionó tiene el tag "MainCamera"
        if (collision.gameObject.CompareTag("MainCamera"))
        {
            // Comprobar si ha pasado suficiente tiempo desde la última rotación
            if (Time.time - lastRotationTime >= cooldownTime)
            {
                // Rotar el objeto asignado en el eje Y
                objectToRotate.Rotate(0, rotationDegrees, 0);
                // Reproducir SFX de que hubo movimiento.
                movedSFX.Play();
                // Actualizar el tiempo de la última rotación
                lastRotationTime = Time.time;
            }
        }
    }
}
