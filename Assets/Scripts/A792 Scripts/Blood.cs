using UnityEngine;
using System.Collections;

public class Blood : MonoBehaviour {

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
        //bloodRB.AddForce((player.transform.position - gameObject.transform.position).normalized * .35f, ForceMode.VelocityChange);
        gameObject.transform.LookAt(player.transform.position);
        bloodRB.MovePosition(transform.position + transform.forward * (5 * Time.deltaTime));     
    }
}
