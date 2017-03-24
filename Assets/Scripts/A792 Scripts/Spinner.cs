using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour {

    public float spinSpeed;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        // spin
        gameObject.transform.Rotate(Vector3.up * (spinSpeed * Time.deltaTime));
	}
}
