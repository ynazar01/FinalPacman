using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 direction;
    private Vector2 nextDirection; // Store the next direction to move

    // LayerMask for detecting walls
    public LayerMask wallLayer;

    // Audio sources
    public AudioSource movementAudioSource; // Audio source for movement
    public AudioSource pelletEatingAudioSource; // Audio source for eating pellet
    public AudioSource powerPelletAudioSource; // Audio source for eating power pellet
    public AudioSource wallCollisionAudioSource; // Audio source for wall collision

    private bool hasHitWall = false; // Flag to track if Pac-Man has hit a wall

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = Vector2.right; // Start moving right
        nextDirection = Vector2.right; // Initially, PacStudent's next direction is the same
    }

    void Update()
    {
        HandleInput();

        // Update direction if there's no wall in the next direction
        if (CanMoveInDirection(nextDirection))
        {
            direction = nextDirection;
            RotatePacStudent(direction);
        }
    }

    void FixedUpdate()
    {
        // Move Pac-Man if there's no wall in the current direction
        if (CanMoveInDirection(direction))
        {
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

            // Reset wall collision flag since Pac-Man is moving freely
            hasHitWall = false;

            // Play movement audio if it's not already playing
            if (movementAudioSource != null && !movementAudioSource.isPlaying)
            {
                movementAudioSource.loop = true;
                movementAudioSource.Play();
            }
        }
        else
        {
            // Play wall collision sound only once when Pac-Man hits the wall
            if (!hasHitWall)
            {
                PlayWallCollisionSound();
                hasHitWall = true; // Set flag to true so the sound only plays once
            }

            // Stop the movement sound when Pac-Man is not moving
            if (movementAudioSource != null && movementAudioSource.isPlaying)
            {
                movementAudioSource.Stop();
            }
        }
    }

    // Handle input for movement and store the next direction if possible
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (CanMoveInDirection(Vector2.up))
                nextDirection = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (CanMoveInDirection(Vector2.down))
                nextDirection = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (CanMoveInDirection(Vector2.left))
                nextDirection = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (CanMoveInDirection(Vector2.right))
                nextDirection = Vector2.right;
        }
    }

    // Check if Pac-Man can move in the desired direction
    private bool CanMoveInDirection(Vector2 dir)
    {
        // Perform a raycast to check if there's a wall in the direction
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 0.6f, wallLayer);
        return hit.collider == null; // Return true if no wall is hit
    }

    // Play wall collision sound when Pac-Man hits a wall
    private void PlayWallCollisionSound()
    {
        if (wallCollisionAudioSource != null && !wallCollisionAudioSource.isPlaying)
        {
            wallCollisionAudioSource.Play();
        }
    }

    // Teleport Pac-Man to a new position and set his new direction
    public void TeleportTo(Vector2 newPosition, Vector2 newDirection)
    {
        // Move Pac-Man to the new position
        rb.position = newPosition;

        // Set the new movement direction after teleporting
        direction = newDirection;

        // Optional: You can update Pac-Man's rotation to face the new direction
        RotatePacStudent(newDirection);

        Debug.Log("Teleported to position: " + newPosition + " with new direction: " + newDirection);
    }

    // Rotate Pac-Man to face the direction he is moving
    private void RotatePacStudent(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Method to be called when Pac-Man eats a pellet
    public void EatPellet()
    {
        // Play pellet-eating sound
        if (pelletEatingAudioSource != null)
        {
            pelletEatingAudioSource.Play();
        }
        else
        {
            Debug.LogError("Pellet Eating AudioSource is not assigned in the Inspector or is null!");
        }
    }

    // Method to be called when Pac-Man eats a power pellet
    public void EatPowerPellet()
    {
        // Play power pellet-eating sound
        if (powerPelletAudioSource != null)
        {
            powerPelletAudioSource.Play();
        }
        else
        {
            Debug.LogError("Power Pellet AudioSource is not assigned in the Inspector or is null!");
        }
    }
}
