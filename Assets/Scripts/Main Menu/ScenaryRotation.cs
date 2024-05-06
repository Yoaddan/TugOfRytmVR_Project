using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenaryRotation : MonoBehaviour
{
    public float rotationSpeed = 50.0f;  // Velocidad de rotación en grados por segundo

    void Update()
    {
        // Rotar el objeto alrededor del eje Y en cada frame
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
