using UnityEngine;
using System.Collections;

public class GuardAnimationTest : MonoBehaviour {

    Animator anim;
    Rigidbody rb;
    Rigidbody[] limbRBs;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        limbRBs = GetComponentsInChildren<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetBool("isWalking", true);
            anim.SetBool("isFiring", false);
            anim.SetBool("isThrowing", false);
            anim.SetBool("isAiming", false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isFiring", false);
            anim.SetBool("isThrowing", false);
            anim.SetBool("isAiming", true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isFiring", true);
            anim.SetBool("isThrowing", false);
            anim.SetBool("isAiming", true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isFiring", false);
            anim.SetBool("isThrowing", true);
            anim.SetBool("isAiming", true);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            foreach(Rigidbody tempRig in limbRBs)
            {
                tempRig.velocity = Vector3.zero;
            }
            rb.velocity = Vector3.zero;
            rb.isKinematic = false;
            Destroy(anim);
        }
    }
}
