using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
	[Header("Timer Settings")]
	[Tooltip("How long the round lasts, in seconds. 60 = one minute.")]
	public float timeLimit = 60f;

	[Tooltip("If true, the timer starts counting the moment the scene loads.")]
	public bool autoStart = true;

	[Header("UI (optional)")]
	[Tooltip("Drag a 'TextMeshPro - Text (UI)' here to show the countdown. Leave empty for no display.")]
	public TextMeshProUGUI timerText;

	[Tooltip("When fewer than this many seconds remain, the text switches to the warning colour.")]
	public float warningThreshold = 10f;

	public Color normalColor = Color.white;
	public Color warningColor = Color.red;

	[Header("What happens when time runs out")]
	[Tooltip("Hook up anything here: show a panel, play a sound, call a GameManager method, etc.")]
	public UnityEvent onTimeUp;

	[Tooltip("Optional: scene to load when time runs out. Leave empty to do nothing here.")]
	public string sceneToLoadOnTimeUp = "Menu";

	// --- internal state ---
	private float timeRemaining;
	private bool isRunning;
	private bool hasFinished;

	/// Seconds left on the clock. Read-only for other scripts.
	public float TimeRemaining => timeRemaining;
	public bool IsRunning => isRunning;
	public bool HasFinished => hasFinished;

	void Start()
	{
		timeRemaining = timeLimit;
		UpdateDisplay();

		if (autoStart)
			StartTimer();
	}

	void Update()
	{
		if (!isRunning || hasFinished) return;

		// Time.deltaTime respects Time.timeScale, so pausing the game
		// (timeScale = 0) also pauses this timer automatically.
		timeRemaining -= Time.deltaTime;

		if (timeRemaining <= 0f)
		{
			timeRemaining = 0f;
			UpdateDisplay();
			TimeUp();
			return;
		}

		UpdateDisplay();
	}

	// ---------- Public controls (call from other scripts or UI Buttons) ----------

	/// Begin (or resume from the start logic) counting down.
	public void StartTimer()
	{
		hasFinished = false;
		isRunning = true;
	}

	/// Pause the countdown without resetting it.
	public void PauseTimer() => isRunning = false;

	/// Resume a paused countdown.
	public void ResumeTimer()
	{
		if (!hasFinished) isRunning = true;
	}

	/// Reset the clock back to the full time limit (stopped).
	public void ResetTimer()
	{
		timeRemaining = timeLimit;
		hasFinished = false;
		isRunning = false;
		UpdateDisplay();
	}

	/// Stop the timer for good. Call this when the player WINS before time runs
	/// out, so the "time up" logic doesn't fire afterwards.
	public void StopTimer()
	{
		isRunning = false;
		hasFinished = true;
	}

	/// Add seconds to the clock (use a negative value to subtract).
	public void AddTime(float seconds)
	{
		timeRemaining = Mathf.Max(0f, timeRemaining + seconds);
		UpdateDisplay();
	}

	// ---------------------------- Internals ----------------------------

	private void TimeUp()
	{
		if (hasFinished) return;   // safety: make sure this only runs once
		hasFinished = true;
		isRunning = false;

		Debug.Log("GameTimer: time is up - finishing the round.");

		// 1. Tell anything wired in the Inspector that time ran out.
		onTimeUp?.Invoke();

		// 2. Optionally load a scene as well.
		if (!string.IsNullOrEmpty(sceneToLoadOnTimeUp))
			SceneManager.LoadScene(sceneToLoadOnTimeUp);
	}

	private void UpdateDisplay()
	{
		if (timerText == null) return;

		// CeilToInt makes the display feel natural: it shows "01:00" for the
		// first second and only hits "00:00" exactly when time is truly up.
		int totalSeconds = Mathf.CeilToInt(timeRemaining);
		int minutes = totalSeconds / 60;
		int seconds = totalSeconds % 60;

		timerText.text = $"{minutes:00}:{seconds:00}";
		timerText.color = timeRemaining <= warningThreshold ? warningColor : normalColor;
	}
}