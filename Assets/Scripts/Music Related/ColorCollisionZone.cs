using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCollisionZone : MonoBehaviour
{
    public Material expectedMaterial; // Asigna esto desde el Inspector a BlueNoteMaterial para la zona azul

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Entró una colisión.");
        Note note = collision.gameObject.GetComponent<Note>();
         if (note != null)
        {
            Debug.Log("Material de la nota: " + note.GetComponent<Renderer>().material.name);
            Debug.Log("Material esperado: " + expectedMaterial.name);
            
            if (note.GetComponent<Renderer>().material.name.Contains(expectedMaterial.name))
            {
                Debug.Log("Cumplió con ser nota del color correspondiente.");
                note.HandleHit();
            }
        }
    }
}

