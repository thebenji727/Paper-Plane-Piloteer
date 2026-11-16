using UnityEngine;

/// <summary>
/// ScriptableObject that defines the flight characteristics of a plane variant.
/// Create assets via Assets > Create > Parcel Plane > Plane Variant
/// Assign one to each plane prefab's PaperPlaneController.
/// </summary>
[CreateAssetMenu(fileName = "NewPlaneVariant", menuName = "Parcel Plane/Plane Variant")]
public class PlaneVariant : ScriptableObject
{
    [Header("Identity")]
    public string displayName = "Standard Plane";
    [TextArea(2, 4)]
    public string description = "Balanced paper plane.";

    [Header("Physics Tuning")]
    [Tooltip("Mass of the Rigidbody (affects inertia and how forces feel)")]
    public float mass = 0.5f;

    [Tooltip("How much lift the wings generate. Higher = climbs more / glides longer")]
    public float liftCoefficient = 1.2f;

    [Tooltip("Base drag (air resistance). Lower = flies farther / faster")]
    public float dragCoefficient = 0.35f;

    [Tooltip("How quickly rotation damps. Higher = more stable / flies straighter")]
    public float angularDamping = 1.5f;

    [Tooltip("Extra upward bias force (helps climbers)")]
    public float climbBias = 0f;

    [Tooltip("How strongly the plane tries to align with its velocity (stability). Higher = straighter flight")]
    public float directionalStability = 2f;

    [Header("Control Feel")]
    [Tooltip("Strength of mid-flight pitch / bank corrections")]
    public float controlAuthority = 8f;

    [Tooltip("Max angle of attack before stall (degrees)")]
    public float stallAngle = 25f;

    [Header("Optional")]
    [Tooltip("Multiplier on launch power (applied by SlingShot if it supports it)")]
    public float launchPowerMultiplier = 1f;
}
