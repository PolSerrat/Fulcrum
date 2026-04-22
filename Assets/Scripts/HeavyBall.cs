using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HeavyBall : MonoBehaviour
{
    [Header("Gravity Settings")]
    [Tooltip("Extra downward force to stop the ball from floating.")]
    public float extraGravity = 10f;

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
    }
}