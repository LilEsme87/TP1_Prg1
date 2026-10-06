using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Transform carryPoint;
    private PickableBox carriedBox;

    private float pickupRange;
    private float dropDistance;
    private Vector3 lastDirection;

    private void Start()
    {
        pickupRange = 2f;     // distancia máxima para recoger
        dropDistance = 1.2f;  // a qué distancia delante del jugador se suelta
        lastDirection = Vector3.forward;

        // Punto de transporte: un hijo del jugador llamado "CarryPoint".
        // Si no existe, se crea arriba del jugador.
        carryPoint = transform.Find("CarryPoint");
        if (carryPoint == null)
        {
            GameObject point = new GameObject("CarryPoint");
            point.transform.SetParent(transform);
            point.transform.localPosition = new Vector3(0f, 1.2f, 0f);
            carryPoint = point.transform;
        }
    }

    private void Update()
    {
        // Recuerda hacia dónde miraba el jugador para soltar la caja delante
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        if (input != Vector3.zero) lastDirection = input.normalized;

        if (Input.GetKeyDown(KeyCode.E) && carriedBox == null)
            TryPickUp();
        else if (Input.GetKeyDown(KeyCode.Q) && carriedBox != null)
            DropBox();
    }

    private void TryPickUp()
    {
        // Busca la caja más cercana dentro del rango
        Collider[] nearby = Physics.OverlapSphere(transform.position, pickupRange);
        PickableBox closest = null;
        float closestDistance = float.MaxValue;

        foreach (Collider c in nearby)
        {
            PickableBox box = c.GetComponent<PickableBox>();
            if (box == null || box.IsCarried) continue;

            float distance = Vector3.Distance(transform.position, box.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = box;
            }
        }

        if (closest != null)
        {
            closest.PickUp(carryPoint);
            carriedBox = closest;
        }
    }

    private void DropBox()
    {
        Vector3 dropPosition = transform.position + lastDirection * dropDistance + Vector3.up * 0.3f;
        carriedBox.Drop(dropPosition);
        carriedBox = null;
    }
}
