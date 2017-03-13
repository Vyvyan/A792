using UnityEngine;
using System.Collections;

public class GuardParts : MonoBehaviour
{

    Guard parentScript;

	// Use this for initialization
	void Start ()
    {
        parentScript = gameObject.GetComponentInParent<Guard>();
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            if (other.relativeVelocity.magnitude > 5)
            {
                parentScript.TakeDamage(2);
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
	    
	}   
}
