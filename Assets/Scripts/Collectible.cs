using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public string name = "";
    public float throwForce = 10f;
    
    [HideInInspector] public bool inHand = false;

    void Start()
    {
        if (!inHand)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.detectCollisions = true;
            rb.useGravity = true;
        }        
    }
}
