using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public string targetTag = "Player";

    [Header("Behind-Plane Settings")]
    public float distance = 12f;
    public float height = 2.5f;
    public float positionSmooth = 12f;   // higher = sticks closer during turns
    public float rotationSmooth = 15f;   // how fast the camera rotates to match
    public float lookAhead = 4f;

    void Start()
    {
        if (target == null)
        {
            GameObject plane = GameObject.FindGameObjectWithTag(targetTag);
            if (plane != null)
                target = plane.transform;
        }

        // Snap into place immediately
        if (target != null)
            SnapBehind();
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Desired position: always directly behind the plane
        Vector3 behindOffset = -target.forward * distance + Vector3.up * height;
        Vector3 desiredPosition = target.position + behindOffset;

        // Smooth position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSmooth * Time.deltaTime);

        // Smooth rotation so it turns with the plane
        Vector3 lookTarget = target.position + target.forward * lookAhead;
        Quaternion desiredRotation = Quaternion.LookRotation(lookTarget - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmooth * Time.deltaTime);
    }

    void SnapBehind()
    {
        Vector3 behindOffset = -target.forward * distance + Vector3.up * height;
        transform.position = target.position + behindOffset;
        transform.LookAt(target.position + target.forward * lookAhead);
    }
}