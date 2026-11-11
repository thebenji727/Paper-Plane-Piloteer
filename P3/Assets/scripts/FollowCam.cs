using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;
    public float camZ;

    void Awake()
    {
        camZ = this.transform.position.z;
    }

    void FixedUpdate()
    {
        if (POI == null) return;
        Vector3 destination = POI.transform.position;
        if (POI != null) 
        {
            Rigidbody poiRigid = POI.GetComponent<Rigidbody>();
            if (poiRigid != null)
            {
                if (poiRigid.IsSleeping())
                {
                    POI = null;
                }
            }
        }
        if (POI != null)
        {
            Debug.Log("POI: " + POI.name);
            destination.x = Mathf.Max(minXY.x, destination.x);
            destination.y = Mathf.Max(minXY.x, destination.y);
            destination = Vector3.Lerp(transform.position, destination, easing);
            destination.z = camZ;
            transform.position = destination;
            Camera.main.orthographicSize = destination.y + 10;
        }
        else
        {
            Debug.Log("No POI");
            destination = new Vector3(0, 0, camZ);
            transform.position = destination;
        }

    }
}
