using UnityEngine;

/// <summary>
/// Moves the DeliveryZone to a random position in front of the slingshot
/// while keeping it on the same Z axis and on the ground.
/// Compatible with Unity 2022.3+.
/// </summary>
public class RandomizeDeliveryZone : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("The DeliveryZone to move. Leave empty to move this GameObject.")]
    public Transform deliveryZone;

    [Header("Slingshot")]
    public Transform slingshot;

    [Header("Spawn Settings")]
    [Tooltip("Minimum distance in front (along X)")]
    public float minDistance = 25f;

    [Tooltip("Maximum distance in front (along X)")]
    public float maxDistance = 120f;

    [Tooltip("Ground Y height")]
    public float groundHeight = -10f;

    [Tooltip("Lock Z to the slingshot's Z so the zone stays on the same depth plane")]
    public bool lockToSlingshotZ = true;

    [Header("Behavior")]
    public bool randomizeOnStart = true;
    public bool resetDeliveryTarget = true;

    void Awake()
    {
        if (deliveryZone == null)
            deliveryZone = transform;

        if (slingshot == null)
        {
            var found = GameObject.Find("Slingshot");
            if (found != null)
                slingshot = found.transform;
        }
    }

    void Start()
    {
        if (randomizeOnStart)
            Randomize();
    }

    public void Randomize()
    {
        if (deliveryZone == null || slingshot == null)
        {
            Debug.LogError("[RandomizeDeliveryZone] Missing deliveryZone or slingshot.", this);
            return;
        }

        // Always move forward along positive X from the slingshot
        float distance = Random.Range(minDistance, maxDistance);
        float x = slingshot.position.x + distance;
        float y = groundHeight;
        float z = lockToSlingshotZ ? slingshot.position.z : deliveryZone.position.z;

        Vector3 newPos = new Vector3(x, y, z);
        deliveryZone.position = newPos;

        // Pull any child visual down so it doesn't float
        foreach (Transform child in deliveryZone)
        {
            child.localPosition = new Vector3(child.localPosition.x, 0f, child.localPosition.z);
        }

        if (resetDeliveryTarget)
        {
            var target = deliveryZone.GetComponentInChildren<DeliveryTarget>();
            if (target != null)
                target.ResetTarget();
        }

        Debug.Log($"[RandomizeDeliveryZone] Zone placed at {newPos}", this);
    }

    void OnDrawGizmosSelected()
    {
        if (slingshot == null) return;

        float z = lockToSlingshotZ ? slingshot.position.z : 0f;
        Vector3 near = new Vector3(slingshot.position.x + minDistance, groundHeight, z);
        Vector3 far = new Vector3(slingshot.position.x + maxDistance, groundHeight, z);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(near, far);
        Gizmos.DrawWireCube((near + far) * 0.5f, new Vector3(maxDistance - minDistance, 2f, 2f));
    }
}
