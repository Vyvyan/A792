using UnityEngine;
using System.Collections;

public class CombatRoomDoorway : MonoBehaviour {

    GameObject forceField;

	// Use this for initialization
	void Start ()
    {
        forceField = gameObject.transform.GetChild(2).gameObject;
        forceField.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void TurnOnForceField()
    {
        if (forceField)
        {
            forceField.SetActive(true);
        }
    }

    public void TurnOffForceField()
    {
        if (forceField)
        {
            forceField.SetActive(false);
        }
    }
}
