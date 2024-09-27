using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For handling UI components like Image

public class GameManager : MonoBehaviour
{
    public static GameManager instance;  // Singleton instance
    public int score = 0;                // Game score
    public int lives = 3;                // PacStudent lives
    public float powerModeDuration = 10f; // Duration of power-up mode

    // References to heart icons in the UI
    public Image heart1;                 // First heart icon
    public Image heart2;                 // Second heart icon
    public Image heart3;                 // Third heart icon

    public Text scoreText;               // UI Text to display the score
    public GameObject bananaPrefab;      // Banana prefab for bonus score
    public int bananaScore = 100;        // Score for collecting the banana

    private int totalPellets;
    private int pelletsEaten = 0;
    private bool isPowerMode = false;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of GameManager exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Count total number of pellets in the scene at the start
        totalPellets = FindObjectsByType<Pellet>(FindObjectsSortMode.None).Length;

        // Initialize the UI text with the current score and lives
        UpdateScoreUI();
        UpdateLivesUI();
    }

    // Function to increase score
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    // Function to update the score UI
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    // Function to update the lives UI
    private void UpdateLivesUI()
    {
        // Add null checks to avoid errors if the heart images are not assigned
        if (heart1 == null || heart2 == null || heart3 == null)
        {
            Debug.LogError("Heart icons are not assigned in the Inspector!");
            return;
        }

        // Update heart icons based on lives remaining
        if (lives >= 3)
        {
            heart1.enabled = true;
            heart2.enabled = true;
            heart3.enabled = true;
        }
        else if (lives == 2)
        {
            heart1.enabled = true;
            heart2.enabled = true;
            heart3.enabled = false;
        }
        else if (lives == 1)
        {
            heart1.enabled = true;
            heart2.enabled = false;
            heart3.enabled = false;
        }
        else
        {
            heart1.enabled = false;
            heart2.enabled = false;
            heart3.enabled = false;
        }
    }

    // Function to be called when PacStudent loses a life
    public void LoseLife()
    {
        lives--;
        UpdateLivesUI();

        if (lives <= 0)
        {
            GameOver();
        }
    }

    // Function to handle game over
    private void GameOver()
    {
        Debug.Log("Game Over!");
        // Implement game over logic (e.g., reset the level or go to the main menu)
    }

    // Function to be called when PacStudent eats a pellet
    public void PelletEaten()
    {
        pelletsEaten++;
        AddScore(10); // Example score for eating a pellet
        CheckGameWin();
    }

    // Function to activate power-up mode
    public void ActivatePowerMode()
    {
        if (!isPowerMode)
        {
            isPowerMode = true;
            AddScore(50); // Example score for eating a power pellet
            StartCoroutine(PowerModeTimer());
        }
    }

    // Coroutine to handle power-up mode duration
    private IEnumerator PowerModeTimer()
    {
        Debug.Log("Power Mode Activated");
        // Activate frightened ghost mode here (e.g., change ghost behavior)
        yield return new WaitForSeconds(powerModeDuration);
        Debug.Log("Power Mode Ended");
        // End frightened ghost mode
        isPowerMode = false;
    }

    // Function to check if the game is won
    private void CheckGameWin()
    {
        if (pelletsEaten >= totalPellets)
        {
            Debug.Log("All pellets eaten! You win!");
            // Implement win condition (e.g., end the game, restart level, etc.)
        }
    }

    // Function to spawn a bonus banana on the map
    public void SpawnBanana(Vector2 position)
    {
        Instantiate(bananaPrefab, position, Quaternion.identity);
    }

    // Function to be called when PacStudent collects the bonus banana
    public void CollectBanana()
    {
        AddScore(bananaScore);  // Add bonus score
        Debug.Log("Bonus Banana Collected! Score: " + bananaScore);
    }
}
