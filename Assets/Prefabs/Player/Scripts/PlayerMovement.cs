using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private float velocity;
    private float acceleration;
    private float jumpForce;
    private float groundCheckDistance;

    private Rigidbody rb;
    private Collider col;
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
        rb.constraints = RigidbodyConstraints.FreezeRotation; // evita que se caiga rodando

        velocity = 5f;
        acceleration = 30f;
        jumpForce = 6f;
        groundCheckDistance = 0.15f;

        stats = new PlayerStats(velocity, acceleration);
        SetMovementStrategy(new AccelerateMovement());
        // SetMovementStrategy(new SmoothMovement());
    }

    public void SetMovementStrategy(IMovementStrategy strategy)
    {
        movementStrategy = strategy;
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
    }

    private bool IsGrounded()
    {
        Bounds b = col.bounds;
        return Physics.Raycast(b.center, Vector3.down, b.extents.y + groundCheckDistance);
    }
}