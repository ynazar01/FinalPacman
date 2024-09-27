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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = Vector2.right; // Start moving right
        nextDirection = Vector2.right; // Initially, PacStudent's next direction is the same
    }

    void Update()
    {
        // Get input from arrow keys (or WASD) and store it as the next direction
        if (Input.GetKeyDown(KeyCode.UpArrow) && !IsWallInDirection(Vector2.up))
        {
            nextDirection = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && !IsWallInDirection(Vector2.down))
        {
            nextDirection = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !IsWallInDirection(Vector2.left))
        {
            nextDirection = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !IsWallInDirection(Vector2.right))
        {
            nextDirection = Vector2.right;
        }

        // If there is no wall in the next direction, set it as the current direction
        if (!IsWallInDirection(nextDirection))
        {
            direction = nextDirection;
            RotatePacStudent(direction);
        }
    }

    void FixedUpdate()
    {
        // Move only if there are no walls in the current direction
        if (!IsWallInDirection(direction))
        {
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
    }

    // Raycast to detect walls
    private bool IsWallInDirection(Vector2 dir)
    {
        // Send a raycast from PacStudent's position in the given direction
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 0.6f, wallLayer);
        // If the raycast hits a wall (layer is wallLayer), return true
        return hit.collider != null;
    }

    // Rotate PacStudent based on movement direction
    private void RotatePacStudent(Vector2 movementDirection)
    {
        float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg; // Get the angle
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // Rotate PacStudent
    }

    // Method to teleport Pacman when entering a passage
    public void TeleportTo(Vector2 newPosition, Vector2 newDirection)
    {
        // Move Pacman to the new position
        transform.position = newPosition;

        // Set the new direction for Pacman after teleporting
        SetDirection(newDirection);
    }

    // Method to set Pacman's direction manually
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
        nextDirection = newDirection;
        RotatePacStudent(newDirection);  // Ensure Pacman faces the new direction
    }
}
