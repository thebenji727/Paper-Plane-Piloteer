using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Detects when a paper plane successfully delivers.
/// Uses both trigger collision and a proximity fallback so it works even if colliders don't overlap perfectly.
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
    public ZoneType zoneType = ZoneType.Doormat;
    public bool useCustomBaseScore = false;
    public int customBaseScore = 100;

    [Header("Scoring")]
    public float perfectSoftSpeed = 3f;
    public float hardCrashSpeed = 12f;

    [Header("Detection")]
    [Tooltip("Also count as delivered if the plane gets this close and is moving slowly")]
    public float proximityRadius = 8f;
    public float proximityMaxSpeed = 6f;

    [Header("Events & Feedback")]
    public UnityEvent<int> onDelivery;
    public ParticleSystem deliveryEffect;

    [Header("Plane Handling")]
    public bool freezePlaneAfterDelivery = true;
    public float freezeDelay = 1.2f;

    [Header("Debug")]
    public bool showDebugLog = true;

    private Collider zoneCollider;
    private bool alreadyDelivered;

    void Awake()
    {
        zoneCollider = GetComponent<Collider>();
        if (zoneCollider != null && !zoneCollider.isTrigger)
        {
            Debug.LogWarning($"{name}: DeliveryTarget collider is not a Trigger. Forcing isTrigger = true.", this);
            zoneCollider.isTrigger = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        TryDeliver(other);
    }

    void OnTriggerStay(Collider other)
    {
        TryDeliver(other);
    }

    void Update()
    {
        if (alreadyDelivered) return;

        // Proximity fallback — catches cases where the visual zone is big but the collider is small
        var planes = FindObjectsOfType<PaperPlaneController>();
        foreach (var plane in planes)
        {
            if (plane == null) continue;

            float dist = Vector3.Distance(transform.position, plane.transform.position);
            Rigidbody rb = plane.GetComponent<Rigidbody>();
            float speed = rb != null ? rb.velocity.magnitude : 0f;

            if (dist <= proximityRadius && speed <= proximityMaxSpeed)
            {
                if (showDebugLog)
                    Debug.Log($"[DeliveryTarget] Proximity delivery! Distance: {dist:F1} Speed: {speed:F1}", this);

                alreadyDelivered = true;
                HandleDelivery(plane);
                break;
            }
        }
    }

    void TryDeliver(Collider other)
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
        float softness = 1f - Mathf.InverseLerp(perfectSoftSpeed, hardCrashSpeed, speed);
        softness = Mathf.Clamp01(softness);

        int finalScore = Mathf.RoundToInt(baseScore * (0.5f + 0.5f * softness));
        int stars = softness > 0.75f ? 3 : softness > 0.4f ? 2 : 1;

        if (showDebugLog)
        {
            Debug.Log($"[DeliveryTarget] {name} ({zoneType}) delivered! " +
                      $"Speed: {speed:F1} | Softness: {softness:P0} | Score: {finalScore} | Stars: {stars}", this);
        }

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

    public void ResetTarget()
    {
        alreadyDelivered = false;
    }

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
    }
}
