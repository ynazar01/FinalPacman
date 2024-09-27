using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    // Set the layer number for Pacman in the Inspector
    public LayerMask pacmanLayer; 

    // This function is called when another object enters the pellet's trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is Pacman by checking its layer
        if (((1 << other.gameObject.layer) & pacmanLayer) != 0)
        {
            // Pacman has collided with this pellet, so destroy the pellet
            Destroy(gameObject);
            
            // Optionally, trigger any additional logic like increasing score here
            // GameManager.instance.AddScore(10); // Example of adding score
        }
    }
}
