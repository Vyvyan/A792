using UnityEngine;
using System.Collections;

public class A792_Player : MonoBehaviour {

    public GameObject centerAimObject;
    Camera cam;
    public Transform rightEye, leftEye;
    [Header("Projectile Types")]
    public GameObject pistolProjectile;

	// Use this for initialization
	void Start ()
    {
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Ray centerAimRay = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit centerAimRayHit;

        if (Physics.Raycast(centerAimRay, out centerAimRayHit, 200))
        {
            centerAimObject.transform.position = centerAimRayHit.point;
        }
        else
        {
            centerAimObject.transform.position = (cam.transform.position + (cam.transform.forward * 100));
        }

        if (Input.GetMouseButtonDown(0))
        {
            // object to spawn
            GameObject tempShot = Instantiate(pistolProjectile, leftEye.position, Quaternion.identity) as GameObject;
            // direction to aim target
            Vector3 dir = (centerAimObject.transform.position - leftEye.position).normalized;
            // add force
            tempShot.GetComponent<Rigidbody>().AddForce(dir * 70, ForceMode.VelocityChange);
        }

        if (Input.GetMouseButtonDown(1))
        {
            // object to spawn
            GameObject tempShot = Instantiate(pistolProjectile, rightEye.position, Quaternion.identity) as GameObject;
            // direction to aim target
            Vector3 dir = (centerAimObject.transform.position - rightEye.position).normalized;
            // add force
            tempShot.GetComponent<Rigidbody>().AddForce(dir * 70, ForceMode.VelocityChange);
        }
    }
}
