using UnityEngine;
using System.Collections;

public class CombatRoom_FloorTrigger : MonoBehaviour {

    CombatRoom parentCombatRoomScript;
    Collider thisTrigger;

	// Use this for initialization
	void Start ()
    {
        parentCombatRoomScript = gameObject.GetComponentInParent<CombatRoom>();
        thisTrigger = gameObject.GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (parentCombatRoomScript.isInCombat)
        {
            thisTrigger.enabled = false;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("a player stepped on us");
            parentCombatRoomScript.isInCombat = true;
        }
    }
}
