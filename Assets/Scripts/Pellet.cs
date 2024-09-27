using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    public LayerMask pacmanLayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is Pacman by checking its layer
        if (((1 << other.gameObject.layer) & pacmanLayer) != 0)
        {
            // Pacman has collided with this pellet, so destroy the pellet
            Destroy(gameObject);

            // Try to play pellet eating sound if available
            PacStudentMovement pacStudentMovement = other.GetComponent<PacStudentMovement>();
            if (pacStudentMovement != null)
            {
                pacStudentMovement.EatPellet();
            }

            // Notify the GameManager that a pellet has been eaten
            if (GameManager.instance != null)
            {
                GameManager.instance.PelletEaten();
            }
        }
    }
}
