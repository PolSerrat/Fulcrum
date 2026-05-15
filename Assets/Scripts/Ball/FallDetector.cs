using UnityEngine;
using UnityEngine.SceneManagement; // CRITICAL: This line allows us to change scenes

public class FallDetector : MonoBehaviour
{
    [Tooltip("The height at which the ball is considered 'fallen off'")]
    public float fallThreshold = -50f; // Adjust this in the Inspector

    void Update()
    {
        // Check if the ball's Y position has dropped below our threshold
        if (transform.position.y < fallThreshold)
        {
            // Load the menu scene
            SceneManager.LoadScene("Menu");
        }
    }
}