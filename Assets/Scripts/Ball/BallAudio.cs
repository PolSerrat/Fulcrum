using UnityEngine;

// This ensures your ball has the necessary components so you don't get errors
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class BallAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private Rigidbody rb;

    [Header("Audio Settings")]
    [Tooltip("How fast the ball needs to move to play the sound. Increase this if it plays when barely moving.")]
    public float speedThreshold = 0.1f;

    [Tooltip("How often to play the sound in seconds.")]
    public float playInterval = 1.0f;

    private float timer;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        // Ensure 3D sound is on and auto-looping is off (since our code handles the looping)
        audioSource.spatialBlend = 1f;
        audioSource.loop = false;

        // Set the timer to the interval so the sound plays immediately the moment it starts rolling
        timer = playInterval;
    }

    void Update()
    {
        // 1. Check current speed of the ball
        float currentSpeed = rb.linearVelocity.magnitude;

        // 2. Is the ball moving fast enough to be considered "rolling a lot"?
        if (currentSpeed > speedThreshold)
        {
            // Advance the timer by the amount of time that has passed since the last frame
            timer += Time.deltaTime;

            // 3. Has 1 second passed?
            if (timer >= playInterval)
            {
                audioSource.Play(); // Play the sound
                timer = 0f;         // Reset the timer back to 0
            }
        }
        else
        {
            // If the ball stops, we stop the audio immediately
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // Reset the timer so it's ready to play the exact moment it starts moving fast again
            timer = playInterval;
        }
    }
}