using UnityEngine;
using UnityEngine.SceneManagement;

public class FallDetector : MonoBehaviour
{
	[Tooltip("The height at which the ball is considered 'fallen off'")]
	public float fallThreshold = -50f; // Adjust this in the Inspector

	[Header("Game Manager Reference")]
	[Tooltip("Optional - found automatically if left empty.")]
	public GameManager gameManager;

	private bool hasFallen = false;

	void Start()
	{
		if (gameManager == null)
		{
			gameManager = FindFirstObjectByType<GameManager>();
		}
	}

	void Update()
	{
		// Only ever trigger once.
		if (hasFallen) return;

		// Has the ball's Y position dropped below our threshold?
		if (transform.position.y < fallThreshold)
		{
			hasFallen = true;

			// Trigger the shared defeat screen (freeze + defeat text + defeat music, then Menu).
			if (gameManager != null)
			{
				gameManager.GameLost();
			}
			else
			{
				// Fallback if there is no GameManager in the scene.
				SceneManager.LoadScene("Menu");
			}
		}
	}
}