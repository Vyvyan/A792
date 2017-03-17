using UnityEngine;
using System.Collections;

public class Blood : MonoBehaviour {

    //
    /// <summary>
    /// THIS SCRIPT ISN'T USED. IT was originally going to be used like gold in A79, but we dont use it anymore
    /// </summary>

    GameObject player;
    Rigidbody bloodRB;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bloodRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.LookAt(player.transform.position);
        bloodRB.MovePosition(transform.position + transform.forward * (5 * Time.deltaTime));     
    }
}
