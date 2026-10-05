using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 target;
    private float speed;
    private float waitTime;
    private bool isMoving;

    private Collider col;
    private HashSet<Transform> riders; // quienes están parados encima

    private void Start()
    {
        // Si la plataforma tiene Rigidbody, lo dejamos sin gravedad
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        col = GetComponent<Collider>();
        riders = new HashSet<Transform>();

        speed = 2f;      // unidades por segundo (lento)
        waitTime = 2f;   // segundos de pausa en cada extremo

        pointA = transform.position;
        pointB = pointA + new Vector3(6f, 0f, 0f); // 6 unidades a la derecha
        target = pointB;
        isMoving = true;

        Debug.Log("MovingPlatform activa en: " + gameObject.name);
    }

    private void Update()
    {
        if (!isMoving) return;

        Vector3 oldPos = transform.position;
        transform.position = Vector3.MoveTowards(oldPos, target, speed * Time.deltaTime);
        Vector3 delta = transform.position - oldPos;

        // Lleva consigo a quien esté encima
        foreach (Transform rider in riders)
            rider.position += delta;

        // Llegó al extremo: pausa y cambio de dirección con Invoke()
        if (Vector3.Distance(transform.position, target) < 0.001f)
        {
            isMoving = false;
            Invoke(nameof(ChangeDirection), waitTime);
        }
    }

    // Llamado por Invoke() luego de la espera
    private void ChangeDirection()
    {
        target = (target == pointB) ? pointA : pointB;
        isMoving = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        // Solo cuenta si está parado ENCIMA (no si choca de costado)
        if (collision.collider.bounds.min.y >= col.bounds.max.y - 0.15f)
            riders.Add(collision.transform);
        else
            riders.Remove(collision.transform);
    }

    private void OnCollisionExit(Collision collision)
    {
        riders.Remove(collision.transform);
    }
}