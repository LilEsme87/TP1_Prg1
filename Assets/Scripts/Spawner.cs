using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Este es el único campo público: Unity necesita que le indiques
    // qué prefab instanciar arrastrándolo en el Inspector.
    public GameObject obstaclePrefab;

    private float startDelay;     // segundos antes del primer obstáculo
    private float interval;       // segundos entre obstáculos
    private float lifeTime;       // segundos hasta que el obstáculo se elimina
    private float launchSpeed;    // velocidad inicial con la que sale
    private Vector3 launchDirection;

    private void Start()
    {
        if (obstaclePrefab == null)
        {
            Debug.LogWarning("Spawner: falta asignar el prefab del obstáculo.");
            return;
        }

        startDelay = 1f;
        interval = 2f;
        lifeTime = 6f;
        launchSpeed = 4f;
        launchDirection = transform.forward; // gira el Spawner para apuntar hacia otro lado

        // Genera un obstáculo cada 'interval' segundos
        InvokeRepeating(nameof(SpawnObstacle), startDelay, interval);
    }

    private void SpawnObstacle()
    {
        GameObject obstacle = Instantiate(obstaclePrefab, transform.position, Quaternion.identity);

        // Lo lanza para que se mueva (interacción física perceptible)
        Rigidbody rb = obstacle.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddForce(launchDirection.normalized * launchSpeed, ForceMode.VelocityChange);

        // Evita la acumulación indefinida: se elimina pasado el tiempo
        Destroy(obstacle, lifeTime);
    }
}