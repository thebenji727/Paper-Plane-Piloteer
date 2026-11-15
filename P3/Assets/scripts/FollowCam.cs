using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public static GameObject POI;
    public float easing = 0.08f;
    public Vector2 minXY = Vector2.zero;

    float camZ;
    Vector3 slingshotView;

    void Awake()
    {
        camZ = transform.position.z;
        var ss = FindObjectOfType<SlingShot>();
        slingshotView = ss ? ss.transform.position + new Vector3(0, 1f, 0) : Vector3.zero;
    }

    void FixedUpdate()
    {
        Vector3 dest;

        if (POI != null)
        {
            dest = POI.transform.position;
            dest.x = Mathf.Max(minXY.x, dest.x);
            dest.y = Mathf.Max(minXY.y, dest.y);
            dest.z = camZ;
            transform.position = Vector3.Lerp(transform.position, dest, easing);

            float size = Mathf.Max(10f, dest.y + 8f);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, size, easing);
        }
        else
        {
            dest = slingshotView;
            dest.z = camZ;
            transform.position = Vector3.Lerp(transform.position, dest, easing);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 10f, easing);
        }
    }
}