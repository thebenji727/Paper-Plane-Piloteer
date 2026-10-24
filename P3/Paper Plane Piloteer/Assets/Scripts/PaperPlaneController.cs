using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PaperPlaneController : MonoBehaviour
{
    [Header("Flight Forces")]
    public float thrustForce = 4f;
    public float liftForce = 1.2f;          // lowered so it doesn't climb as aggressively
    public float pitchTorque = 3f;
    public float rollTorque = 2.5f;
    public float yawTorque = 1.5f;

    [Header("Slow Down (Ctrl)")]
    public float brakeDrag = 3.5f;          // extra drag while holding Ctrl
    public float normalDrag = 1.2f;
    public float brakeThrustMultiplier = 0.1f; // almost no thrust while braking

    [Header("Input")]
    public float pitchSensitivity = 1f;
    public float rollSensitivity = 1f;

    private Rigidbody rb;
    private float pitchInput;
    private float rollInput;
    private float thrustInput;
    private bool isBraking;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 0.12f;
        rb.drag = normalDrag;
        rb.angularDrag = 2f;
    }

    void Update()
    {
        pitchInput = Input.GetAxis("Vertical");
        rollInput = -Input.GetAxis("Horizontal");

        isBraking = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        // Less passive thrust; almost none while braking
        float baseThrust = isBraking ? brakeThrustMultiplier : 0.15f;
        thrustInput = Input.GetKey(KeyCode.Space) ? 1f : baseThrust;

        // Apply extra drag while braking
        rb.drag = isBraking ? brakeDrag : normalDrag;
    }

    void FixedUpdate()
    {
        // Forward thrust
        rb.AddForce(transform.forward * thrustForce * thrustInput);

        // Lift (weaker so it doesn't rise too fast)
        float speed = rb.velocity.magnitude;
        if (speed > 0.1f)
        {
            float angleOfAttack = Vector3.Dot(transform.up, rb.velocity.normalized);
            // Smaller boost so nose-up doesn't rocket you upward
            rb.AddForce(Vector3.up * liftForce * speed * Mathf.Clamp01(angleOfAttack + 0.1f));
        }

        // Rotational control
        rb.AddTorque(transform.right * pitchInput * pitchTorque * pitchSensitivity);
        rb.AddTorque(transform.forward * rollInput * rollTorque * rollSensitivity);

        // Mild damping
        rb.AddTorque(-rb.angularVelocity * 0.8f);
    }
}