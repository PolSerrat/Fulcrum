using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HeavyBall : MonoBehaviour
{
    [Header("Gravity Settings")]
    [Tooltip("Extra downward force to stop the ball from floating.")]
    public float extraGravity = 10f;
    
    [Header("Speed Settings")]
    [Tooltip("Velocidad máxima que puede alcanzar la bola")]
    public float maxSpeed = 50f;
    
    public float speed = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // ForceMode.Acceleration applies the force regardless of the ball's mass,
        // acting exactly like custom gravity.
        rb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);

        // Limitar velocidad máxima
        float currentSpeed = rb.linearVelocity.magnitude;
        if (currentSpeed > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        // Incrementar la velocidad de la bola
        rb.AddForce(rb.linearVelocity.normalized * speed, ForceMode.Acceleration);
    }
}