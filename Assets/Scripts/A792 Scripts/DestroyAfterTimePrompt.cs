using UnityEngine;
using System.Collections;

public class DestroyAfterTimePrompt : MonoBehaviour
{
    public float countdownToDestroy;
    public bool countingDown;

	// Use this for initialization
	void Start ()
    {
        countingDown = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*       
	    if (countingDown)
        {
            countdownToDestroy -= Time.deltaTime;
        }

        if (countdownToDestroy <= 0)
        {
            Destroy(gameObject);
        }
        */
	}
}
