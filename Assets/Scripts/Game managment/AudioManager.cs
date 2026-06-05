using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music Clips")]
    public AudioClip backgroundMusic;
    public AudioClip victoryMusic;
    public AudioClip defeatMusic;
    public AudioClip loseLife;

    private AudioSource musicSource;

    void Start()
    {
        // Crear AudioSource para música
        musicSource = GetComponent<AudioSource>();

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        PlayBackgroundMusic();
    }

    void PlayBackgroundMusic()
    {
        if (backgroundMusic == null)
        {
            Debug.LogWarning("Background music missing");
            return;
        }

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();

        Debug.Log("Background music started");
    }

    public void PlayVictoryMusic()
    {
        if (victoryMusic == null)
        {
            Debug.LogWarning("Victory music missing");
            return;
        }

        musicSource.Stop();

        musicSource.clip = victoryMusic;
        musicSource.loop = false;
        musicSource.Play();

        Debug.Log("Victory music started");
    }

    public void PlayDefeatMusic()
    {
        if (defeatMusic == null)
        {
            Debug.LogWarning("Defeat music missing");
            return;
        }

        musicSource.Stop();

        musicSource.clip = defeatMusic;
        musicSource.loop = false;
        musicSource.Play();

        Debug.Log("Defeat music started");
    }

    public void PlayLoseLifeSound()
    {
        if (loseLife == null)
        {
            Debug.LogWarning("Lose life sound missing");
            return;
        }

        // Instead of looking for a camera, just play the sound exactly where the AudioManager is!
        musicSource.PlayOneShot(loseLife);

        Debug.Log("Lose life sound played");
    }

    public void PauseMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
        }
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }
}