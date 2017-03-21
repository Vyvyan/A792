using UnityEngine;
using System.Collections;

public class IfCollideDestroyOther : MonoBehaviour {

    // This script was made for the combat room, outer rooms, to see if when they get spawned they are on top of another room.
    
	// Use this for initialization
	void Start ()
    {
        StartCoroutine(WaitThenDestroy());

        // check to see if there's preexisting room where we just spawned
        Ray rayCheck = new Ray(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 10, gameObject.transform.position.z), Vector3.down);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(rayCheck, 50);

        if (hits.Length > 2)
        {
            Destroy(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    IEnumerator WaitThenDestroy()
    {
        yield return new WaitForSeconds(.1f);
        Destroy(gameObject.GetComponent<SphereCollider>());
        Destroy(this);
    }
}
