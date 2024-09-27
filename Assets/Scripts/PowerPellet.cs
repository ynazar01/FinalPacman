using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPellet : MonoBehaviour
{
    // Set the layer number for Pacman in the Inspector
    public LayerMask pacmanLayer; // Field for Pacman layer

    // This function is called when another object enters the power pellet's trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is Pacman by checking its layer
        if (((1 << other.gameObject.layer) & pacmanLayer) != 0)
        {
            // Pacman has collided with this power pellet, so destroy the power pellet
            Destroy(gameObject);

            // Play the power pellet eating sound
            PacStudentMovement pacStudentMovement = other.GetComponent<PacStudentMovement>();
            if (pacStudentMovement != null)
            {
                pacStudentMovement.EatPowerPellet();
            }

            // Optionally, trigger any additional logic like activating power mode here
            // GameManager.instance.ActivatePowerMode(); // Example of activating power-up mode
        }
    }
}
