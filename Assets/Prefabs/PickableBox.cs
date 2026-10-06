using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickableBox : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private bool isCarried;

    private Vector3 startPosition;
    private float fallLimit;

    public bool IsCarried => isCarried;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        startPosition = transform.position;
        fallLimit = startPosition.y - 15f;
    }

    // Recoger: se hace hijo del punto de transporte del jugador
    public void PickUp(Transform carryPoint)
    {
        if (isCarried) return;
        isCarried = true;

        rb.isKinematic = true;   // sin física propia mientras se carga
        col.enabled = false;     // no choca con el jugador ni con los obstáculos

        transform.SetParent(carryPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    // Soltar: recupera su independencia jerárquica y su física
    public void Drop(Vector3 worldPosition)
    {
        if (!isCarried) return;
        isCarried = false;

        transform.SetParent(null);
        transform.position = worldPosition;

        col.enabled = true;
        rb.isKinematic = false;
        rb.velocity = Vector3.zero; // en Unity anterior a 6: rb.velocity
    }

    // Si la caja cae al vacío, vuelve a su lugar original
    private void FixedUpdate()
    {
        if (isCarried) return;

        if (transform.position.y < fallLimit)
        {
            rb.velocity = Vector3.zero; // en Unity anterior a 6: rb.velocity
            rb.angularVelocity = Vector3.zero;
            transform.position = startPosition;
        }
    }
}
