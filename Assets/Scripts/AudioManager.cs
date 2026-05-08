using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Background Music")]
    public AudioClip backgroundMusic;

    [Header("Victory Music")]
    public AudioClip victoryMusic;
    
    private AudioSource audioSource;

    void Start()
    {
        // Crear un AudioSource si no existe
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configurar el AudioSource
        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
            Debug.Log("Música de fondo iniciada");
        }
        else
        {
            Debug.LogWarning("AudioManager: Background Music no está asignado en el Inspector");
        }
    }

    // Se llama desde GameManager cuando ganas
    public void PlayVictoryMusic()
    {
        if (audioSource == null) return;

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (victoryMusic != null)
        {
            audioSource.clip = victoryMusic;
            audioSource.loop = false;
            audioSource.Play();
            Debug.Log("Música de victoria iniciada");
        }
        else
        {
            Debug.LogWarning("AudioManager: Victory Music no está asignado en el Inspector");
        }
    }

    // Opcional: pausar música sin detenerla completamente
    public void PauseMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    // Opcional: reanudar música
    public void ResumeMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
