using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passage : MonoBehaviour
{
    // Assign the exit position of the passage in the Inspector
    public Transform exitPoint;

    // LayerMask for detecting Pacman
    public LayerMask pacmanLayer;

    // Cooldown duration to prevent immediate re-teleportation
    public float teleportCooldown = 0.5f;

    // Deactivation duration for the exit passage to prevent teleporting back
    public float passageDeactivationDuration = 1f;

    // Store the last time Pacman teleported
    private float lastTeleportTime;

    // Define whether this passage teleports Pacman left or right
    public bool isRightSide;  // If true, this is the right passage; if false, it's the left passage

    // Offset to teleport Pacman a little outside the exit point to prevent immediate retrigger
    public float teleportOffsetX = 0.5f;  // Fine-tuned X offset

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is Pacman (using the layer)
        if (((1 << other.gameObject.layer) & pacmanLayer) != 0)
        {
            // Ensure cooldown has passed before allowing another teleport
            if (Time.time > lastTeleportTime + teleportCooldown)
            {
                // Get the PacStudentMovement script from Pacman
                PacStudentMovement pacmanMovement = other.GetComponent<PacStudentMovement>();

                if (pacmanMovement != null)
                {
                    // Determine which side Pacman is coming from and set the correct direction
                    Vector2 newDirection;
                    Vector2 offsetDirection;

                    if (isRightSide)
                    {
                        // Pacman is teleporting from the right side, so after teleport he faces left
                        newDirection = Vector2.left;
                        offsetDirection = new Vector2(-teleportOffsetX, 0);  // Fine-tuned left offset
                    }
                    else
                    {
                        // Pacman is teleporting from the left side, so after teleport he faces right
                        newDirection = Vector2.right;
                        offsetDirection = new Vector2(teleportOffsetX, 0);  // Fine-tuned right offset
                    }

                    // Calculate the new teleport position by applying the offset
                    Vector2 newTeleportPosition = (Vector2)exitPoint.position + offsetDirection;

                    // Teleport Pacman to the new position and set direction
                    pacmanMovement.TeleportTo(newTeleportPosition, newDirection);

                    // Deactivate the exit passage temporarily to prevent instant re-teleportation
                    StartCoroutine(DeactivatePassageTemporarily(exitPoint.gameObject));  // Deactivate the exit passage only

                    // Update the last teleport time to prevent immediate re-teleportation
                    lastTeleportTime = Time.time;
                }
            }
        }
    }

    // Coroutine to deactivate the exit passage temporarily
    IEnumerator DeactivatePassageTemporarily(GameObject passage)
    {
        passage.SetActive(false);  // Deactivate the exit passage
        yield return new WaitForSeconds(passageDeactivationDuration);  // Wait for the deactivation duration
        passage.SetActive(true);  // Reactivate the passage
    }
}
