using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameOverUI : MonoBehaviour
{
	[Header("Panels")]
	public GameObject victoryPanel;
	public GameObject defeatPanel;

	[Header("Victory UI Elements")]
	public TextMeshProUGUI victoryTitleText;
	public TextMeshProUGUI victoryMessageText;
	public TextMeshProUGUI victoryTimeText;

	[Header("Defeat UI Elements")]
	public TextMeshProUGUI defeatTitleText;
	public TextMeshProUGUI defeatMessageText;

	[Header("Animation Settings")]
	public float fadeInDuration = 0.5f;

	void Awake()
	{
		if (victoryPanel != null) victoryPanel.SetActive(false);
		if (defeatPanel != null) defeatPanel.SetActive(false);
	}

	// Called by GameManager when the player wins
	public void ShowVictoryScreen(float timeRemaining)
	{
		if (victoryPanel == null)
		{
			Debug.LogWarning("GameOverUI: VictoryPanel is not assigned!");
			return;
		}

		if (victoryTitleText != null)
			victoryTitleText.text = "¡GANASTE!";

		if (victoryMessageText != null)
			victoryMessageText.text = "Has guiado la bola hasta la meta.";

		if (victoryTimeText != null)
		{
			int totalSeconds = Mathf.CeilToInt(timeRemaining);
			int minutes = totalSeconds / 60;
			int seconds = totalSeconds % 60;
			victoryTimeText.text = "Tiempo restante: " + $"{minutes:00}:{seconds:00}";
		}

		victoryPanel.SetActive(true);
		StartCoroutine(FadeInPanel(victoryPanel));
	}

	// Called by GameManager when the player loses
	public void ShowDefeatScreen(string reason)
	{
		if (defeatPanel == null)
		{
			Debug.LogWarning("GameOverUI: DefeatPanel is not assigned!");
			return;
		}

		if (defeatTitleText != null)
			defeatTitleText.text = "PERDISTE";

		if (defeatMessageText != null)
		{
			switch (reason)
			{
				case "lives":
					defeatMessageText.text = "Te quedaste sin vidas.";
					break;
				case "time":
					defeatMessageText.text = "Se acabó el tiempo.";
					break;
				case "fall":
					defeatMessageText.text = "La bola cayó al vacío.";
					break;
				default:
					defeatMessageText.text = "Inténtalo de nuevo.";
					break;
			}
		}

		defeatPanel.SetActive(true);
		StartCoroutine(FadeInPanel(defeatPanel));
	}

	// --- Button callbacks (wire in Inspector OnClick) ---

	public void OnRetryButton()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void OnMenuButton()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("Menu");
	}

	public void OnNextLevelButton()
	{
		Time.timeScale = 1f;
		int currentIndex = SceneManager.GetActiveScene().buildIndex;
		int nextIndex = currentIndex + 1;

		if (nextIndex < SceneManager.sceneCountInBuildSettings)
			SceneManager.LoadScene(nextIndex);
		else
			SceneManager.LoadScene("Menu");
	}

	// Smooth fade-in (works even when Time.timeScale = 0)
	private IEnumerator FadeInPanel(GameObject panel)
	{
		CanvasGroup cg = panel.GetComponent<CanvasGroup>();
		if (cg == null)
			cg = panel.AddComponent<CanvasGroup>();

		cg.alpha = 0f;
		float elapsed = 0f;

		while (elapsed < fadeInDuration)
		{
			elapsed += Time.unscaledDeltaTime;
			cg.alpha = Mathf.Clamp01(elapsed / fadeInDuration);
			yield return null;
		}

		cg.alpha = 1f;
	}
}