using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMovement : MonoBehaviour
{
    public float speed = 5.0f;         // Velocidad de movimiento de la roca
    private Vector3 startPosition;     // Posición inicial de la roca
    private Vector3 targetPosition;    // Posición objetivo de la roca (donde estaba el jugador al spawnear la roca)
    private float startTime;           // Momento en que la roca comienza a moverse
    private float journeyLength;       // Distancia total del recorrido de la roca
    private RockManager rockManager; // Referencia al RockManager
    private HealthBar healthBar;  // Referencia a la barra de salud

    void Start()
    {
        startPosition = transform.position;
        // Intentar encontrar el RockManager si no se ha asignado
        if (rockManager == null)
        {
            rockManager = FindObjectOfType<RockManager>();
        }
        // Buscar el componente HealthBar en la escena
        if (healthBar == null)
        {
            healthBar = FindObjectOfType<HealthBar>();
        }
        // Encontrar el jugador por tag y obtener su posición
        GameObject player = GameObject.FindGameObjectWithTag("MainCamera");
        if (player != null)
        {
            targetPosition = player.transform.position;
        }
        else
        {
            Debug.LogError("Jugador no se encontró. Asigna el 'MainCamera' tag.");
            return;
        }

        startTime = Time.time;
        journeyLength = Vector3.Distance(startPosition, targetPosition);
    }

    void Update()
    {
        if (journeyLength > 0)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

            // Comprobar si la roca ha llegado a su destino
            if (fractionOfJourney >= 1.0f)
            {
                rockManager.PlayRockDestroySound();
                Destroy(gameObject);  // Destruir la roca al final de su recorrido
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Comprobar si la roca colisiona con el jugador
        if (collision.gameObject.CompareTag("MainCamera"))
        {
            Debug.Log("Colisioné con el jugador.");
            rockManager.PlayRockDestroySound();
            healthBar.TakeDamage(20);  // Resta 20 puntos de salud
            Destroy(gameObject);  // Destruir la roca al colisionar con el jugador
        }
    }
}
