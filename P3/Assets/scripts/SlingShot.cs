using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlingShot : MonoBehaviour
{
    public GameObject launchPoint;
    public GameObject projectilePrefab;
    public float velocityMult = 10f;
    public GameObject projLinePrefab;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    private void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPoint.transform.position;
    }

    private void OnMouseEnter()
    {
        if (IsPointerOverUI()) return;
        launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (IsPointerOverUI()) return;
        if (projectilePrefab == null) return;

        // Destroy previous plane
        if (projectile != null) Destroy(projectile);

        aimingMode = true;
        projectile = Instantiate(projectilePrefab);
        // Spawn a little higher so it doesn't clip into the ground
        projectile.transform.position = launchPos + Vector3.up * 0.5f;
        projectile.transform.rotation = Quaternion.Euler(0, 90, 0);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;
    }

    private void Update()
    {
        if (!aimingMode || projectile == null) return;

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 mouseDelta = mousePos3D - launchPos;

        float maxMagnitude = GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        projectile.transform.position = launchPos + mouseDelta + Vector3.up * 0.5f;

        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;

            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            if (projRB != null)
                StartCoroutine(LaunchPlane(projRB, mouseDelta));

            // Tell the camera to follow this plane
            FollowCam.POI = projectile;

            if (projLinePrefab != null)
                Instantiate(projLinePrefab, projectile.transform);

            projectile = null;
        }
    }

    private IEnumerator LaunchPlane(Rigidbody rb, Vector3 mouseDelta)
    {
        rb.isKinematic = false;
        yield return null;
        yield return new WaitForFixedUpdate();

        if (rb != null)
        {
            Vector3 vel = -mouseDelta * velocityMult * 4f;
            vel.y = Mathf.Max(vel.y, 6f);          // stronger upward kick
            rb.velocity = vel;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}