using UnityEngine;
using System.Collections;

public class MrTootsParts : MonoBehaviour {

    public MrToots parentScript;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            if (other.relativeVelocity.magnitude > 5)
            {
                parentScript.TakeDamage(2, other.gameObject.transform.position);
                other.gameObject.tag = "Untagged";
            }
        }
    }
}
