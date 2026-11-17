using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Attach to any GameObject that has a Collider set as a Trigger.
/// When a PaperPlaneController enters the volume, it registers a successful delivery
/// and calculates a score based on landing softness (lower speed = higher score).
///
/// Compatible with Unity 2022.3+.
/// </summary>
[RequireComponent(typeof(Collider))]
public class DeliveryTarget : MonoBehaviour
{
    public enum ZoneType
    {
        Bushes = 50,
        Railing = 100,
        Stoop = 200,
        Doormat = 300
    }

    [Header("Zone")]
    [Tooltip("What kind of landing zone this is (affects base score)")]
    public ZoneType zoneType = ZoneType.Doormat;

    [Tooltip("Override the base score from the zone type if you want a custom value")]
    public bool useCustomBaseScore = false;
    public int customBaseScore = 100;

    [Header("Scoring")]
    [Tooltip("Speed at or below this counts as a perfect soft landing")]
    public float perfectSoftSpeed = 3f;

    [Tooltip("Speed at or above this counts as a hard crash (minimum score multiplier)")]
    public float hardCrashSpeed = 12f;

    [Header("Events & Feedback")]
    [Tooltip("Invoked when a plane successfully delivers. Passes the final score.")]
    public UnityEvent<int> onDelivery;

    [Tooltip("Optional: a particle system or other effect to play on delivery")]
    public ParticleSystem deliveryEffect;

    [Header("Plane Handling")]
    [Tooltip("If true, the plane's Rigidbody is made kinematic after a short delay so it settles")]
    public bool freezePlaneAfterDelivery = true;
    public float freezeDelay = 1.2f;

    [Header("Debug")]
    public bool showDebugLog = true;

    private Collider zoneCollider;
    private bool alreadyDelivered;

    void Awake()
    {
        zoneCollider = GetComponent<Collider>();
        if (!zoneCollider.isTrigger)
        {
            Debug.LogWarning($"{name}: DeliveryTarget collider is not a Trigger. Forcing isTrigger = true.", this);
            zoneCollider.isTrigger = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (alreadyDelivered) return;

        var plane = other.GetComponentInParent<PaperPlaneController>();
        if (plane == null) return;

        alreadyDelivered = true;
        HandleDelivery(plane);
    }

    void HandleDelivery(PaperPlaneController plane)
    {
        Rigidbody rb = plane.GetComponent<Rigidbody>();
        float speed = rb != null ? rb.velocity.magnitude : 0f;

        int baseScore = useCustomBaseScore ? customBaseScore : (int)zoneType;

        // Softness: 1.0 at perfectSoftSpeed or below, 0.0 at hardCrashSpeed or above
        float softness = 1f - Mathf.InverseLerp(perfectSoftSpeed, hardCrashSpeed, speed);
        softness = Mathf.Clamp01(softness);

        // Final score: base * (0.5 + 0.5 * softness) so even a hard landing still scores something
        int finalScore = Mathf.RoundToInt(baseScore * (0.5f + 0.5f * softness));

        int stars = softness > 0.75f ? 3 : softness > 0.4f ? 2 : 1;

        if (showDebugLog)
        {
            Debug.Log($"[DeliveryTarget] {name} ({zoneType}) delivered! " +
                      $"Speed: {speed:F1} | Softness: {softness:P0} | Score: {finalScore} | Stars: {stars}", this);
        }

        // Fire event so other systems (UI, progression, sound) can react
        onDelivery?.Invoke(finalScore);

        if (deliveryEffect != null)
            deliveryEffect.Play();

        if (freezePlaneAfterDelivery && rb != null)
            StartCoroutine(FreezePlaneAfterDelay(rb));
    }

    System.Collections.IEnumerator FreezePlaneAfterDelay(Rigidbody rb)
    {
        yield return new WaitForSeconds(freezeDelay);
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }

    // Visual aid in the Scene view
    void OnDrawGizmos()
    {
        Collider col = GetComponent<Collider>();
        if (col == null) return;

        Gizmos.color = alreadyDelivered ? new Color(0.2f, 0.8f, 0.2f, 0.35f) : new Color(1f, 0.85f, 0.1f, 0.35f);
        Gizmos.matrix = transform.localToWorldMatrix;

        if (col is BoxCollider box)
            Gizmos.DrawCube(box.center, box.size);
        else if (col is SphereCollider sphere)
            Gizmos.DrawSphere(sphere.center, sphere.radius);
        else
            Gizmos.DrawCube(Vector3.zero, Vector3.one); // fallback
    }

    void OnDrawGizmosSelected()
    {
        Collider col = GetComponent<Collider>();
        if (col == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix;

        if (col is BoxCollider box)
            Gizmos.DrawWireCube(box.center, box.size);
        else if (col is SphereCollider sphere)
            Gizmos.DrawWireSphere(sphere.center, sphere.radius);
    }

    /// <summary>
    /// Call this if you want to reuse the same target for another attempt (e.g. after reset).
    /// </summary>
    public void ResetTarget()
    {
        alreadyDelivered = false;
    }
}
