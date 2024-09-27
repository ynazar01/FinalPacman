using UnityEngine;

public class Banana : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if PacStudent collected the banana
        if (other.CompareTag("PacStudent"))
        {
            // Call the GameManager to handle banana collection
            GameManager.instance.CollectBanana();

            // Destroy the banana after it is collected
            Destroy(gameObject);
        }
    }
}
