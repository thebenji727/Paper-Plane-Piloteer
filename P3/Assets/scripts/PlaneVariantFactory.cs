using UnityEngine;

/// <summary>
/// Helper that creates the four recommended plane variants at runtime
/// (or you can make them as assets in the Editor and assign manually).
/// Drop this on an empty GameObject in the scene and press Play once,
/// then you can copy the created assets if desired.
/// </summary>
public class PlaneVariantFactory : MonoBehaviour
{
    [ContextMenu("Create Example Variants")]
    public void CreateExampleVariants()
    {
        CreateVariant("Dart_Straight", 
            "Flies almost straight with minimal arch. Fast and stable.",
            mass: 0.8f, lift: 0.45f, drag: 0.22f, angDamp: 3.5f, climb: 0f, stability: 6f, control: 5f, stall: 18f);

        CreateVariant("Glider_Distance", 
            "Long-distance flyer. Excellent glide ratio, floats far.",
            mass: 0.4f, lift: 2.1f, drag: 0.18f, angDamp: 2.0f, climb: 0.3f, stability: 3f, control: 6f, stall: 28f);

        CreateVariant("Climber_High", 
            "Climbs aggressively. Great for reaching tall roofs.",
            mass: 0.45f, lift: 1.8f, drag: 0.32f, angDamp: 1.2f, climb: 2.5f, stability: 1.5f, control: 9f, stall: 32f);

        CreateVariant("Stunt_Agile", 
            "Highly maneuverable but short range. Fun for precise deliveries.",
            mass: 0.35f, lift: 1.0f, drag: 0.55f, angDamp: 0.6f, climb: 0f, stability: 0.8f, control: 14f, stall: 35f);

        CreateVariant("Cargo_Heavy", 
            "Heavy package carrier. Drops faster, short range, very stable.",
            mass: 1.4f, lift: 0.7f, drag: 0.48f, angDamp: 4.0f, climb: -0.5f, stability: 4f, control: 4f, stall: 20f);

        Debug.Log("Created 5 example PlaneVariant assets in Assets/ (if running in Editor). " +
                  "Assign them to different plane prefabs.");
    }

    void CreateVariant(string name, string desc, float mass, float lift, float drag, 
                       float angDamp, float climb, float stability, float control, float stall)
    {
#if UNITY_EDITOR
        PlaneVariant v = ScriptableObject.CreateInstance<PlaneVariant>();
        v.displayName = name.Replace("_", " ");
        v.description = desc;
        v.mass = mass;
        v.liftCoefficient = lift;
        v.dragCoefficient = drag;
        v.angularDamping = angDamp;
        v.climbBias = climb;
        v.directionalStability = stability;
        v.controlAuthority = control;
        v.stallAngle = stall;
        v.launchPowerMultiplier = 1f;

        string path = $"Assets/{name}.asset";
        UnityEditor.AssetDatabase.CreateAsset(v, path);
        UnityEditor.AssetDatabase.SaveAssets();
        Debug.Log($"Created variant: {path}");
#endif
    }
}
