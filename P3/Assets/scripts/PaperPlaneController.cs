using UnityEngine;

/// <summary>
/// Attach this to any plane prefab (along with a Rigidbody).
/// Assign a PlaneVariant asset in the Inspector to give it unique flight behavior.
/// Works with the existing SlingShot launch system.
/// Compatible with Unity 2022.3.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PaperPlaneController : MonoBehaviour
{
    [Header("Variant")]
    [Tooltip("Drag a PlaneVariant asset here to define this plane's flight style")]
    public PlaneVariant variant;

    [Header("Debug")]
    public bool showDebugInfo = false;

    private Rigidbody rb;
    private float currentAoA;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ApplyVariantSettings();
    }

    void ApplyVariantSettings()
    {
        if (variant == null)
        {
            Debug.LogWarning($"{name}: No PlaneVariant assigned. Using defaults.");
            return;
        }

        rb.mass = variant.mass;
        rb.drag = 0.05f;                   // Unity 2022 name (becomes linearDamping in Unity 6)
        rb.angularDrag = variant.angularDamping;
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void FixedUpdate()
    {
        if (variant == null || rb.velocity.sqrMagnitude < 0.1f) return;

        ApplyAerodynamicForces();
        ApplyDirectionalStability();
        ApplyClimbBias();
        HandlePlayerControls();
    }

    void ApplyAerodynamicForces()
    {
        Vector3 velocity = rb.velocity;
        float speed = velocity.magnitude;
        if (speed < 0.5f) return;

        // Local velocity for angle of attack
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        currentAoA = Mathf.Atan2(-localVelocity.y, localVelocity.z) * Mathf.Rad2Deg;

        // Stall check
        bool stalled = Mathf.Abs(currentAoA) > variant.stallAngle;

        // Lift (perpendicular to velocity, upward relative to plane)
        float liftForce = 0f;
        if (!stalled)
        {
            // Simple lift model: proportional to speed² and AoA (clamped)
            float aoaFactor = Mathf.Clamp(currentAoA / variant.stallAngle, -1f, 1f);
            liftForce = variant.liftCoefficient * speed * speed * aoaFactor * 0.15f;
        }

        // Lift direction: cross of right and velocity (roughly "up" relative to flight path)
        Vector3 liftDirection = Vector3.Cross(transform.right, velocity.normalized);
        if (liftDirection.sqrMagnitude > 0.01f)
            rb.AddForce(liftDirection.normalized * liftForce);

        // Drag (opposes velocity)
        float dragForce = variant.dragCoefficient * speed * speed * 0.05f;
        rb.AddForce(-velocity.normalized * dragForce);
    }

    void ApplyDirectionalStability()
    {
        if (variant.directionalStability <= 0f) return;

        Vector3 velocityDir = rb.velocity.normalized;
        Vector3 currentForward = transform.forward;

        // Align forward with velocity (yaw + mild pitch)
        Vector3 correction = Vector3.Cross(currentForward, velocityDir);
        rb.AddTorque(correction * variant.directionalStability * rb.velocity.magnitude * 0.08f);

        // Extra leveling: push the nose down if the plane is pitched too high
        // (prevents the looping you saw on the Dart)
        float pitchAngle = Vector3.Angle(transform.forward, Vector3.ProjectOnPlane(transform.forward, Vector3.up));
        if (transform.forward.y > 0.15f) // nose is up
        {
            float levelStrength = variant.directionalStability * 0.4f;
            Vector3 levelTorque = -transform.right * levelStrength * transform.forward.y;
            rb.AddTorque(levelTorque);
        }
    }

    void ApplyClimbBias()
    {
        if (variant.climbBias != 0f)
        {
            rb.AddForce(Vector3.up * variant.climbBias);
        }
    }

    void HandlePlayerControls()
    {
        // Mid-flight corrections (matches GDD: brief force bursts)
        float pitch = Input.GetAxis("Vertical");   // W/S or Up/Down
        float bank  = Input.GetAxis("Horizontal"); // A/D or Left/Right

        if (Mathf.Abs(pitch) > 0.1f || Mathf.Abs(bank) > 0.1f)
        {
            Vector3 torque = Vector3.zero;
            torque += transform.right * pitch * variant.controlAuthority;     // pitch
            torque += transform.up    * -bank * variant.controlAuthority * 0.7f; // bank/yaw feel

            rb.AddTorque(torque);
        }
    }

    void OnGUI()
    {
        if (!showDebugInfo) return;
        GUILayout.Label($"AoA: {currentAoA:F1}°");
        GUILayout.Label($"Speed: {rb.velocity.magnitude:F1}");
        if (variant != null)
            GUILayout.Label($"Variant: {variant.displayName}");
    }

    // Optional: called by SlingShot if you want variant-specific launch tweaks
    public float GetLaunchPowerMultiplier()
    {
        return variant != null ? variant.launchPowerMultiplier : 1f;
    }
}
