using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class BallAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private Rigidbody rb;

    [Header("Audio Settings")]

    [Tooltip("Minimum speed required to play rolling sound")]
    public float speedThreshold = 0.1f;

    [Tooltip("Time between rolling sounds")]
    public float playInterval = 1.0f;

    private float timer;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        // Configure audio
        audioSource.spatialBlend = 1f; // 3D sound
        audioSource.loop = false;

        timer = playInterval;
    }

    void Update()
    {
        float currentSpeed = rb.linearVelocity.magnitude;

        if (currentSpeed > speedThreshold)
        {
            timer += Time.deltaTime;

            if (timer >= playInterval)
            {
                audioSource.PlayOneShot(audioSource.clip);

                timer = 0f;
            }
        }
        else
        {
            timer = playInterval;
        }
    }
}