using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

    // variables for what to show and what to hide when one of the exits doesn't have a connecting room
    public GameObject up_hide, down_hide, left_hide, right_hide, up_show, down_show, left_show, right_show;
    public bool isCombatRoomMainTile;
    [Header("Tiles")]
    public GameObject TopLeft;
    public GameObject TopRight;
    public GameObject BottomRight;
    public GameObject BottomLeft;

    public GameObject[] topWalls, botWalls, leftWalls, rightWalls;

	// Use this for initialization
	void Start ()
    {
        if (isCombatRoomMainTile)
        {
            ChangeCombatRoomShape(Random.Range(1,5));
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    public void CheckNeighbors()
    {
        // setup mofo combat rooms
        SetupCombatRooms();


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
        /*     
        // the direction codes are like a clock, 1 is up, 2 is right, 3 is down, 4 is left
        if (direction_code == 1)
        {
            if (up_hide)
            {
                up_hide.SetActive(false);
            }
            if (up_show)
            {
                up_show.SetActive(true);
            }
        }
        else if (direction_code == 2)
        {
            if (right_hide)
            {
                right_hide.SetActive(false);
            }
            if (right_show)
            {
                right_show.SetActive(true);
            }
        }
        else if (direction_code == 3)
        {
            if (down_hide)
            {
                down_hide.SetActive(false);
            }
            if (down_show)
            {
                down_show.SetActive(true);
            }
        }
        else if (direction_code == 4)
        {
            if (left_hide)
            {
                left_hide.SetActive(false);
            }
            if (left_show)
            {
                left_show.SetActive(true);
            }
        }
        */

        
        // the direction codes are like a clock, 1 is up, 2 is right, 3 is down, 4 is left
        if (direction_code == 1)
        {
            if (up_hide)
            {
                Destroy(up_hide);
            }
            if (up_show)
            {
                up_show.SetActive(true);
            }
        }
        else if (direction_code == 2)
        {
            if (right_hide)
            {
                Destroy(right_hide);
            }
            if (right_show)
            {
                right_show.SetActive(true);
            }
        }
        else if (direction_code == 3)
        {
            if (down_hide)
            {
                Destroy(down_hide);
            }
            if (down_show)
            {
                down_show.SetActive(true);
            }
        }
        else if (direction_code == 4)
        {
            if (left_hide)
            {
                Destroy(left_hide);
            }
            if (left_show)
            {
                left_show.SetActive(true);
            }
        }
        
    }

    void SetupCombatRooms()
    {
        // NOW FOR COMBAT ROOMS
        if (isCombatRoomMainTile)
        {
            // if we have all of our tiles still alive, then destroy all walls
            if (BottomLeft && TopLeft && TopRight)
            {
                foreach (GameObject wall in topWalls)
                {
                    Destroy(wall);
                }
                foreach (GameObject wall in botWalls)
                {
                    Destroy(wall);
                }
                foreach (GameObject wall in leftWalls)
                {
                    Destroy(wall);
                }
                foreach (GameObject wall in rightWalls)
                {
                    Destroy(wall);
                }
            }
            
            // if they're all gone except us
            else if (!BottomLeft && !TopLeft && !TopRight)
            {
                foreach (GameObject wall in topWalls)
                {
                    Destroy(wall);
                }
                foreach (GameObject wall in leftWalls)
                {
                    Destroy(wall);
                }
            }

            // if there is no top left or bottom left
            else if (!BottomLeft && !TopLeft)
            {
                foreach (GameObject wall in rightWalls)
                {
                    Destroy(wall);
                }
            }

            // if there is no top tiles
            else if (!TopRight && !TopLeft)
            {
                foreach (GameObject wall in botWalls)
                {
                    Destroy(wall);
                }
            }

            // if there is no top left
            else if (!TopLeft)
            {
                foreach (GameObject wall in botWalls)
                {
                    Destroy(wall);
                }
                foreach (GameObject wall in rightWalls)
                {
                    Destroy(wall);
                }
            }

            // if there is no top right
            else if (!TopRight)
            {
                foreach (GameObject wall in botWalls)
                {
                    Destroy(wall);
                }
                foreach (GameObject wall in leftWalls)
                {
                    Destroy(wall);
                }
            }

            // if there is no top left
            else if (!BottomLeft)
            {
                foreach (GameObject wall in topWalls)
                {
                    Destroy(wall);
                }
                foreach (GameObject wall in rightWalls)
                {
                    Destroy(wall);
                }
            }

            
        }
    }

    void ChangeCombatRoomShape(int variant)
    {
        if (variant == 1)
        {
            // one square combat room
            Destroy(TopLeft);
            Destroy(TopRight);
            Destroy(BottomLeft);
        }
        else if (variant == 2)
        {
            // two square horizontal combat room
            Destroy(TopLeft);
            Destroy(TopRight);
        }
        else if (variant == 3)
        {
            // two square vertical combat room
            Destroy(TopLeft);
            Destroy(BottomLeft);
        }
        else if (variant == 4)
        {
            // full
        }
    }
}
