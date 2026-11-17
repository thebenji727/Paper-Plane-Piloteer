using UnityEngine;

/// <summary>
/// Moves a DeliveryZone to a random position inside a user-defined area.
/// Just set the Min/Max values for the spawn volume.
/// Compatible with Unity 2022.3+.
/// </summary>
public class RandomizeDeliveryZone : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("The DeliveryZone parent to move. If left empty, moves this GameObject.")]
    public Transform deliveryZone;

    [Header("Spawn Area")]
    [Tooltip("Minimum corner of the allowed spawn area (world space)")]
    public Vector3 min = new Vector3(10f, -10f, -4f);

    [Tooltip("Maximum corner of the allowed spawn area (world space)")]
    public Vector3 max = new Vector3(200f, -10f, 4f);

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

        // Ensure min is actually smaller than max on each axis
        Vector3 low = Vector3.Min(min, max);
        Vector3 high = Vector3.Max(min, max);

        float x = Random.Range(low.x, high.x);
        float y = Random.Range(low.y, high.y);
        float z = Random.Range(low.z, high.z);

        Vector3 newPos = new Vector3(x, y, z);
        deliveryZone.position = newPos;

        if (resetDeliveryTarget)
        {
            var target = deliveryZone.GetComponentInChildren<DeliveryTarget>();
            if (target != null)
                target.ResetTarget();
        }

        Debug.Log($"[RandomizeDeliveryZone] Moved {deliveryZone.name} to {newPos}", this);
    }

    // Visualize the spawn area in the Scene view
    void OnDrawGizmosSelected()
    {
        Vector3 low = Vector3.Min(min, max);
        Vector3 high = Vector3.Max(min, max);
        Vector3 center = (low + high) * 0.5f;
        Vector3 size = high - low;

        Gizmos.color = new Color(0.3f, 1f, 0.4f, 0.25f);
        Gizmos.DrawCube(center, size);

        Gizmos.color = new Color(0.3f, 1f, 0.4f, 0.8f);
        Gizmos.DrawWireCube(center, size);
    }
}
