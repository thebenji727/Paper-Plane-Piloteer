using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach to a UI Button (or any GameObject) and wire the DoReset() method to the button's OnClick.
/// Resets the current plane and returns the camera to the slingshot view.
/// Disables any FollowCam while returning so the two don't fight.
/// </summary>
public class ResetBoard : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The Main Camera")]
    public Camera mainCamera;

    [Tooltip("The Slingshot object (or empty transform that marks the slingshot camera position)")]
    public Transform slingshotView;

    [Tooltip("Optional: the SlingShot script so we can clear its current projectile")]
    public SlingShot slingShot;

    [Header("Camera Reset")]
    [Tooltip("How fast the camera returns (0 = instant snap)")]
    public float cameraReturnSpeed = 8f;

    [Header("Optional")]
    [Tooltip("If true, also re-opens the plane selection panel after reset")]
    public bool showPlaneSelectorOnReset = false;

    private bool returningCamera;
    private MonoBehaviour followCam;   // any follow script we temporarily disable

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (slingshotView == null && slingShot != null)
            slingshotView = slingShot.transform;

        // Try to find a common follow camera script so we can disable it during reset
        if (mainCamera != null)
        {
            followCam = mainCamera.GetComponent<MonoBehaviour>();
            // Prefer a script that actually has "Follow" in the name
            var scripts = mainCamera.GetComponents<MonoBehaviour>();
            foreach (var s in scripts)
            {
                if (s != null && s.GetType().Name.Contains("Follow"))
                {
                    followCam = s;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Call this from a UI Button's OnClick event.
    /// </summary>
    public void DoReset()
    {
        // 1. Destroy any existing plane
        DestroyExistingPlanes();

        // 2. Clear slingshot projectile reference
        if (slingShot != null)
        {
            var projectileField = slingShot.GetType().GetField("projectile");
            if (projectileField != null)
                projectileField.SetValue(slingShot, null);

            var aimingField = slingShot.GetType().GetField("aimingMode");
            if (aimingField != null)
                aimingField.SetValue(slingShot, false);
        }

        // 3. Disable follow camera so it doesn't fight us
        if (followCam != null)
            followCam.enabled = false;

        // 4. Start returning camera
        returningCamera = true;

        // 5. Optional: re-open plane selector
        if (showPlaneSelectorOnReset)
        {
            var selector = FindObjectOfType<PlaneSelector>();
            if (selector != null)
                selector.ShowPanel();
        }

        Debug.Log("Board reset. Camera returning to slingshot.");
    }

    void DestroyExistingPlanes()
    {
        var planes = FindObjectsOfType<PaperPlaneController>();
        foreach (var plane in planes)
            Destroy(plane.gameObject);
    }

    void Update()
    {
        if (!returningCamera || mainCamera == null || slingshotView == null) return;

        // Smoothly move toward the current slingshot view position
        mainCamera.transform.position = Vector3.Lerp(
            mainCamera.transform.position,
            slingshotView.position,
            Time.deltaTime * cameraReturnSpeed);

        mainCamera.transform.rotation = Quaternion.Slerp(
            mainCamera.transform.rotation,
            slingshotView.rotation,
            Time.deltaTime * cameraReturnSpeed);

        // When close enough, snap and re-enable follow cam
        if (Vector3.Distance(mainCamera.transform.position, slingshotView.position) < 0.1f)
        {
            mainCamera.transform.position = slingshotView.position;
            mainCamera.transform.rotation = slingshotView.rotation;
            returningCamera = false;

            if (followCam != null)
                followCam.enabled = true;
        }
    }
}
