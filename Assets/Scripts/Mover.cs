using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

    UnityEngine.AI.NavMeshAgent agent;

	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
