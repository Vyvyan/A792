using UnityEngine;
using System.Collections;

public class AntiGravBullets : MonoBehaviour {

    Rigidbody rb;

	// Use this for initialization
	void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void OnCollisionEnter(Collision other)
    {
        if (rb)
        {
            if (!rb.useGravity)
            {
                rb.useGravity = true;
            }
        }
    }
}
