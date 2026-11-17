using UnityEngine;

/// <summary>
/// Moves a DeliveryZone (or any transform) to a random position on the ground.
/// Ground defaults: position (0, -10, 0), scale (30000, 1, 10).
/// Attach this to any GameObject (or the DeliveryZone itself) and call Randomize() 
/// from a button, ResetBoard, or on Start.
/// Compatible with Unity 2022.3+.
/// </summary>
public class RandomizeDeliveryZone : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("The DeliveryZone parent to move. If left empty, moves this GameObject.")]
    public Transform deliveryZone;

    [Header("Ground")]
    [Tooltip("World position of the ground center")]
    public Vector3 groundPosition = new Vector3(0f, -10f, 0f);

    [Tooltip("Scale of the ground (used to calculate usable area)")]
    public Vector3 groundScale = new Vector3(30000f, 1f, 10f);

    [Header("Placement")]
    [Tooltip("How far in from the edges the zone is allowed to spawn (prevents hanging off)")]
    public float marginX = 5f;
    public float marginZ = 1f;

    [Tooltip("Extra height above the ground surface (useful if the zone has thickness)")]
    public float heightOffset = 0.1f;

    [Header("Optional Limits")]
    [Tooltip("If true, clamps the random X range (useful so targets stay reachable from the slingshot)")]
    public bool limitXRange = false;
    public float minX = -50f;
    public float maxX = 50f;

    [Header("Behavior")]
    [Tooltip("Randomize automatically when the scene starts")]
    public bool randomizeOnStart = true;

    [Tooltip("Also call ResetTarget() on any DeliveryTarget found under the zone")]
    public bool resetDeliveryTarget = true;

    void Awake()
    {
        if (deliveryZone == null)
            deliveryZone = transform;
    }

    void Start()
    {
        if (randomizeOnStart)
            Randomize();
    }

    /// <summary>
    /// Call this from a UI Button, ResetBoard, or any other script.
    /// </summary>
    public void Randomize()
    {
        if (deliveryZone == null) return;

        // Usable half-extents of the ground (centered at groundPosition)
        float halfX = (groundScale.x * 0.5f) - marginX;
        float halfZ = (groundScale.z * 0.5f) - marginZ;

        halfX = Mathf.Max(0f, halfX);
        halfZ = Mathf.Max(0f, halfZ);

        float x = Random.Range(-halfX, halfX);
        float z = Random.Range(-halfZ, halfZ);

        if (limitXRange)
            x = Mathf.Clamp(x, minX, maxX);

        Vector3 newPos = groundPosition + new Vector3(x, heightOffset, z);
        deliveryZone.position = newPos;

        if (resetDeliveryTarget)
        {
            var target = deliveryZone.GetComponentInChildren<DeliveryTarget>();
            if (target != null)
                target.ResetTarget();
        }

        Debug.Log($"[RandomizeDeliveryZone] Moved {deliveryZone.name} to {newPos}", this);
    }

    // Optional: visualize the usable ground area in the Scene view
    void OnDrawGizmosSelected()
    {
        float halfX = (groundScale.x * 0.5f) - marginX;
        float halfZ = (groundScale.z * 0.5f) - marginZ;
        halfX = Mathf.Max(0f, halfX);
        halfZ = Mathf.Max(0f, halfZ);

        Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.25f);
        Vector3 center = groundPosition + Vector3.up * heightOffset;
        Gizmos.DrawCube(center, new Vector3(halfX * 2f, 0.05f, halfZ * 2f));

        if (limitXRange)
        {
            Gizmos.color = new Color(1f, 0.6f, 0.2f, 0.4f);
            float clampedHalf = Mathf.Min(halfX, Mathf.Max(Mathf.Abs(minX), Mathf.Abs(maxX)));
            Gizmos.DrawWireCube(center, new Vector3(clampedHalf * 2f, 0.1f, halfZ * 2f));
        }
    }
}
