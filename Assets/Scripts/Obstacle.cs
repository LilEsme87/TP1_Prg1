using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerMovement player = collision.collider.GetComponentInParent<PlayerMovement>();
            if (player != null)
                player.ResetToStart();

            Destroy(gameObject);
        }
    }
}