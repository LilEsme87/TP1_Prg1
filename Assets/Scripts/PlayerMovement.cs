using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private float velocity;
    private float acceleration;
    private float jumpForce;
    private float groundCheckDistance;

    private Vector3 startPosition;
    private float fallLimit;

    private Rigidbody rb;
    private Collider col;
    private PlayerInteraction interaction;
    private PlayerStats stats;
    private IMovementStrategy movementStrategy;

    private Vector2 moveInput;
    private bool jumpRequested;

    // Acceso para el Power-Up (cambia stats.Velocity)
    public PlayerStats Stats => stats;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        interaction = GetComponent<PlayerInteraction>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // evita que se caiga rodando

        velocity = 5f;
        acceleration = 30f;
        jumpForce = 6f;
        groundCheckDistance = 0.15f;

        startPosition = transform.position;      // punto de reinicio
        fallLimit = startPosition.y - 15f;       // si cae más abajo, reinicia

        stats = new PlayerStats(velocity, acceleration);
        SetMovementStrategy(new AccelerateMovement());
    }

    public void SetMovementStrategy(IMovementStrategy strategy)
    {
        movementStrategy = strategy;
    }

    // Vuelve al punto de inicio (lo llama el obstáculo o la caída al vacío)
    public void ResetToStart()
    {
        rb.velocity = Vector3.zero; 
        rb.angularVelocity = Vector3.zero;
        transform.position = startPosition;

        // La caja también vuelve a su lugar
        if (interaction != null)
            interaction.ResetBox();
    }

    // Los inputs se leen en Update...
    private void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space))
            jumpRequested = true;
    }

    // ...y la física se aplica en FixedUpdate
    private void FixedUpdate()
    {
        movementStrategy.Move(rb, stats, moveInput);

        if (jumpRequested && IsGrounded())
        {
            Vector3 v = rb.velocity; 
            rb.velocity = new Vector3(v.x, jumpForce, v.z);
        }
        jumpRequested = false;

        if (transform.position.y < fallLimit)
            ResetToStart();
    }

    private bool IsGrounded()
    {
        Bounds b = col.bounds;
        return Physics.Raycast(b.center, Vector3.down, b.extents.y + groundCheckDistance);
    }
}