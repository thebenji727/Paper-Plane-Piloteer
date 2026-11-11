using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Renderer))]

public class Goal : MonoBehaviour
{
    static public bool goalMet = false;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("You can see me");
        Projectile proj = other.GetComponent<Projectile>();
        if ((proj != null)) 
        {
            Debug.Log("You can see me pt. 2");
            Goal.goalMet = true;
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.r = 255f;
            c.g = 0f;
            c.b = 0f;
            mat.color = c;
        }
    }
}
