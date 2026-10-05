using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    private Vector3 offset;
    private float smoothSpeed;
    private float collisionPadding;
    private float minDistance;
    private int obstacleMask;

    private void Start()
    {
        offset = new Vector3(0f, 6f, -7f);
        smoothSpeed = 8f;
        collisionPadding = 0.4f;
        minDistance = 1f;
        obstacleMask = ~0; // todas las capas

        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
        if (player != null) target = player.transform;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 pivot = target.position;
        Vector3 desired = pivot + offset;

        // Si hay algo entre el jugador y la cámara, acercar la cámara
        Vector3 direction = desired - pivot;
        float distance = direction.magnitude;
        direction /= distance;

        float closest = distance;
        RaycastHit[] hits = Physics.RaycastAll(pivot, direction, distance, obstacleMask, QueryTriggerInteraction.Ignore);
        foreach (RaycastHit hit in hits)
        {
            // Ignora al jugador y lo que lleva encima (por ejemplo, la caja)
            if (hit.transform.IsChildOf(target)) continue;
            if (hit.distance < closest) closest = hit.distance;
        }

        if (closest < distance)
            desired = pivot + direction * Mathf.Max(closest - collisionPadding, minDistance);

        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
        transform.LookAt(pivot + Vector3.up);
    }
}
