using UnityEngine;
using UnityEngine.SceneManagement; // CRITICAL: This line allows us to change scenes

public class FallDetector : MonoBehaviour
{
    [Tooltip("The height at which the ball is considered 'fallen off'")]
    public float fallThreshold = -50f; // Adjust this in the Inspector

	[Header("Game Manager")]                           
	public GameManager gameManager;

	void Start()                                             
	{
		if (gameManager == null)
			gameManager = FindFirstObjectByType<GameManager>();
	}

	void Update()
	{
		if (transform.position.y < fallThreshold)
		{
			if (gameManager != null)
				gameManager.GameLost("fall");
			else
				SceneManager.LoadScene("Menu");
		}
	}
}