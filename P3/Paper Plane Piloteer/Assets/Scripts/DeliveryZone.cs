using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    public string requiredTag = "Player"; // or "Package"
    public float maxSpeedForDelivery = 3f;
    public UnityEngine.Events.UnityEvent onDelivered;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(requiredTag)) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb != null && rb.velocity.magnitude > maxSpeedForDelivery)  // was linearVelocity
            return; // Too fast — bounce off or fail

        Debug.Log("Delivery successful!");
        onDelivered?.Invoke();

        // Optional: disable the zone or play feedback
        // gameObject.SetActive(false);
    }
}