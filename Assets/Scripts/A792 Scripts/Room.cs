using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

    // variables for what to show and what to hide when one of the exits doesn't have a connecting room
    public GameObject up_hide, down_hide, left_hide, right_hide, up_show, down_show, left_show, right_show;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    public void CheckNeighbors()
    {
        // UP
        Ray ray_up = new Ray(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 25, gameObject.transform.position.z + 50), Vector3.down);
        RaycastHit upHit;

        // is there a room tile above ours?
        if (Physics.Raycast(ray_up, out upHit))
        {
            if (upHit.collider.gameObject.tag == "Ground")
            {
                // no room above us
                HideRoomParts(1);
            }
        }

        // RIGHT
        Ray ray_right = new Ray(new Vector3(gameObject.transform.position.x + 50, gameObject.transform.position.y + 25, gameObject.transform.position.z), Vector3.down);
        RaycastHit rightHit;

        // is there a room tile to the right of ours?
        if (Physics.Raycast(ray_right, out rightHit))
        {
            if (rightHit.collider.gameObject.tag == "Ground")
            {
                // There's a room beside us
                HideRoomParts(2);
            }
        }

        // DOWN
        Ray ray_down = new Ray(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 25, gameObject.transform.position.z - 50), Vector3.down);
        RaycastHit downHit;

        // is there a room tile above ours?
        if (Physics.Raycast(ray_down, out downHit))
        {
            if (downHit.collider.gameObject.tag == "Ground")
            {
                // There's a room above us
                HideRoomParts(3);
            }
        }

        // LEFT
        Ray ray_left = new Ray(new Vector3(gameObject.transform.position.x - 50, gameObject.transform.position.y + 25, gameObject.transform.position.z), Vector3.down);
        RaycastHit leftHit;

        // is there a room tile above ours?
        if (Physics.Raycast(ray_left, out leftHit))
        {
            if (leftHit.collider.gameObject.tag == "Ground")
            {
                // There's a room beside us
                HideRoomParts(4);
            }
        }
    }

    void HideRoomParts(int direction_code)
    {
        // the direction codes are like a clock, 1 is up, 2 is right, 3 is down, 4 is left
        if (direction_code == 1)
        {
            up_hide.SetActive(false);
            up_show.SetActive(true);
        }
        else if (direction_code == 2)
        {
            right_hide.SetActive(false);
            right_show.SetActive(true);
        }
        else if (direction_code == 3)
        {
            down_hide.SetActive(false);
            down_show.SetActive(true);
        }
        else if (direction_code == 4)
        {
            left_hide.SetActive(false);
            left_show.SetActive(true);
        }
    }
}
