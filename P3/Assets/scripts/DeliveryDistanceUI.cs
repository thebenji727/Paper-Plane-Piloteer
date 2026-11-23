using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Shows a UI distance readout from the camera (or current plane) to the DeliveryZone.
/// Attach to any GameObject and assign a TextMeshPro or legacy Text component.
/// Compatible with Unity 2022.3+.
/// </summary>
public class DeliveryDistanceUI : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The DeliveryZone (or the object moved by RandomizeDeliveryZone). Auto-finds if empty.")]
    public Transform deliveryZone;

    [Tooltip("Optional: the camera to measure from. Defaults to Camera.main.")]
    public Camera mainCamera;

    [Tooltip("If true, prefer measuring from the active PaperPlaneController when one exists.")]
    public bool preferPlaneDistance = true;

    [Header("UI")]
    [Tooltip("TextMeshPro text to update (preferred)")]
    public TMP_Text tmpText;

    [Tooltip("Legacy UI Text (used only if TMP is not assigned)")]
    public Text legacyText;

    [Header("Display")]
    [Tooltip("Format string. {0} = distance number")]
    public string format = "{0:F0} m";

    [Tooltip("Hide the UI when no zone is found")]
    public bool hideWhenMissing = true;

    [Tooltip("Optional: change color when very close")]
    public bool colorByDistance = true;
    public float closeDistance = 15f;
    public Color closeColor = new Color(0.3f, 1f, 0.4f);
    public Color normalColor = Color.white;

    private PaperPlaneController currentPlane;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (deliveryZone == null)
        {
            // Prefer an actual DeliveryTarget if one exists
            var target = FindObjectOfType<DeliveryTarget>();
            if (target != null)
                deliveryZone = target.transform;
            else
            {
                // Fallback: look for a GameObject named DeliveryZone
                var zoneObj = GameObject.Find("DeliveryZone");
                if (zoneObj != null)
                    deliveryZone = zoneObj.transform;
            }
        }

        RefreshPlane();
    }

    void Update()
    {
        if (preferPlaneDistance)
            RefreshPlane();

        Transform from = GetMeasurementTransform();
        bool valid = from != null && deliveryZone != null;

        if (!valid)
        {
            SetVisible(!hideWhenMissing);
            if (!hideWhenMissing)
                SetText("—");
            return;
        }

        SetVisible(true);

        float distance = Vector3.Distance(from.position, deliveryZone.position);
        string label = string.Format(format, distance);
        SetText(label);

        if (colorByDistance)
        {
            Color c = distance <= closeDistance ? closeColor : normalColor;
            SetColor(c);
        }
    }

    void RefreshPlane()
    {
        // Prefer the plane that is currently in flight
        if (currentPlane == null || !currentPlane.gameObject.activeInHierarchy)
            currentPlane = FindObjectOfType<PaperPlaneController>();
    }

    Transform GetMeasurementTransform()
    {
        if (preferPlaneDistance && currentPlane != null)
            return currentPlane.transform;

        if (mainCamera != null)
            return mainCamera.transform;

        return null;
    }

    void SetText(string value)
    {
        if (tmpText != null)
            tmpText.text = value;
        else if (legacyText != null)
            legacyText.text = value;
    }

    void SetColor(Color c)
    {
        if (tmpText != null)
            tmpText.color = c;
        else if (legacyText != null)
            legacyText.color = c;
    }

    void SetVisible(bool visible)
    {
        if (tmpText != null)
            tmpText.gameObject.SetActive(visible);
        else if (legacyText != null)
            legacyText.gameObject.SetActive(visible);
    }
}
