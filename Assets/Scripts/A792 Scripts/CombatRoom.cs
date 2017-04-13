using UnityEngine;
using System.Collections;

public class CombatRoom : MonoBehaviour {

    public bool isInCombat;
    bool hasInitiatedCombat;

    public CombatRoomDoorway[] doors;
    private A792_GameManager gameManager;

    public GameObject enemySpawnerObject;

    [Header("Parent Room World Center Object")]
    public GameObject parentRoomWorldCenterObject;

    [Header("Room Tiles")]
    // we don't need bottom right since that is this game object
    public GameObject topRight, topLeft, bottomLeft;

    public int enemiesToSpawn;
    GameObject[] enemySpawnPoints;
    public float spawnRate, spawnRateCurrent;

	// Use this for initialization
	void Start ()
    {
        isInCombat = false;
        hasInitiatedCombat = false;
        gameManager = FindObjectOfType<A792_GameManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // if we are in combat
	    if (isInCombat)
        {
            // this is the first combat SETUP if you will
            if (!hasInitiatedCombat)
            {
                // we tell the game manager that we are in a combat room, and set the number of enemies we need to kill before the room opens
                A792_GameManager.isFightingInACombatRoom = true;
                A792_GameManager.enemiesLeftInTheCombatRoom = enemiesToSpawn;

                // lock down the room
                LockDownRoom();

                // change the tags temporarily so we don't spawn any enemy spawners inside the room
                gameObject.tag = "Untagged";
                if (topRight) { topRight.tag = "Untagged"; }
                if (topLeft) { topLeft.tag = "Untagged"; }
                if (bottomLeft) { bottomLeft.tag = "Untagged"; }
                
                // now we place our spawners
                PlaceEnemySpawners();

                // we've finished setting up combat, not to switch this off
                hasInitiatedCombat = true;
            }

            // WE'S FIGHTING
            if (enemiesToSpawn > 0)
            {
                if (spawnRateCurrent <= 0)
                {
                    // super simplified spawn code, NEEDS TO BE CHANGED
                    int rnd = Random.Range(1, 3);
                    if (rnd == 1)
                    {
                        // instantiate our enemy (needs to change so we can spawn multiple types of enemies)
                        Instantiate(gameManager.enemy_Guard, enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)].transform.position, Quaternion.identity);
                    }
                    else if (rnd == 2)
                    {
                        // instantiate our enemy (needs to change so we can spawn multiple types of enemies)
                        Instantiate(gameManager.enemy_MrToots, enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)].transform.position, Quaternion.identity);
                    }
                    // tick down the enemies to spawn
                    enemiesToSpawn--;
                    // reset our spawn rate
                    spawnRateCurrent = spawnRate;
                }
                else
                {
                    spawnRateCurrent -= Time.deltaTime;
                }
            }
            else
            {
                // we spawned all the enemies
                // are all the enemies dead though?
                if (A792_GameManager.enemiesLeftInTheCombatRoom <= 0)
                {
                    // all the enemies are dead, so we open the room
                    A792_GameManager.isFightingInACombatRoom = false;
                    // just in case something weird happens to this number, we'll reset it here
                    A792_GameManager.enemiesLeftInTheCombatRoom = 0;
                    // this is the incombat bool for this script, idk what it does, it does stuff im sure.
                    isInCombat = false;
                    // destroy our spawn points
                    foreach (GameObject spawner in enemySpawnPoints)
                    {
                        Destroy(spawner);
                    }
                    // unlock the room
                    UnlockRoom();

                    // change the tags back
                    gameObject.tag = "Room";
                    if (topRight) { topRight.tag = "Room"; }
                    if (topLeft) { topLeft.tag = "Room"; }
                    if (bottomLeft) { bottomLeft.tag = "Room"; }
                }
            }
        }
	}


    // this gets called from our world builder script after all rooms are placed, and after the hallways are configured
    public void ConfigureDoors()
    {
        doors = GetComponentsInChildren<CombatRoomDoorway>();
    }

    public void LockDownRoom()
    {
        foreach (CombatRoomDoorway doorway in doors)
        {
            doorway.TurnOnForceField();
        }

        FindThisRoomsRoomTiles();
        A792_GameManager.FindActiveCombatRooms();
    }

    public void UnlockRoom()
    {
        foreach (CombatRoomDoorway doorway in doors)
        {
            doorway.TurnOffForceField();
        }

        LoseThisRoomsRoomTiles();
        A792_GameManager.ClearActiveCombatRooms();
    }

    void PlaceEnemySpawners()
    {
        // we get the floor triggers, since they are at the center of each of our tiles
        CombatRoom_FloorTrigger[] combatRoomTiles = GetComponentsInChildren<CombatRoom_FloorTrigger>();

        // now to see if there are rooms around our tiles
        foreach (CombatRoom_FloorTrigger cbRoom in combatRoomTiles)
        {
            // UP
            Ray ray_up = new Ray(new Vector3(cbRoom.gameObject.transform.position.x, cbRoom.gameObject.transform.position.y + 25, cbRoom.gameObject.transform.position.z + 50), Vector3.down);
            RaycastHit upHit;

            // is there a room tile above ours?
            if (Physics.Raycast(ray_up, out upHit))
            {
                if (upHit.collider.gameObject.tag == "Room")
                {
                    // There's a room there
                    Instantiate(enemySpawnerObject, upHit.point, Quaternion.identity);
                }
            }

            // RIGHT
            Ray ray_right = new Ray(new Vector3(cbRoom.gameObject.transform.position.x + 50, cbRoom.gameObject.transform.position.y + 25, cbRoom.gameObject.transform.position.z), Vector3.down);
            RaycastHit rightHit;

            // is there a room tile to the right of ours?
            if (Physics.Raycast(ray_right, out rightHit))
            {
                if (rightHit.collider.gameObject.tag == "Room")
                {
                    // There's a room beside us
                    Instantiate(enemySpawnerObject, rightHit.point, Quaternion.identity);
                }
            }

            // DOWN
            Ray ray_down = new Ray(new Vector3(cbRoom.gameObject.transform.position.x, cbRoom.gameObject.transform.position.y + 25, cbRoom.gameObject.transform.position.z - 50), Vector3.down);
            RaycastHit downHit;

            // is there a room tile above ours?
            if (Physics.Raycast(ray_down, out downHit))
            {
                if (downHit.collider.gameObject.tag == "Room")
                {
                    // There's a room above us
                    Instantiate(enemySpawnerObject, downHit.point, Quaternion.identity);
                }
            }

            // LEFT
            Ray ray_left = new Ray(new Vector3(cbRoom.gameObject.transform.position.x - 50, cbRoom.gameObject.transform.position.y + 25, cbRoom.gameObject.transform.position.z), Vector3.down);
            RaycastHit leftHit;

            // is there a room tile above ours?
            if (Physics.Raycast(ray_left, out leftHit))
            {
                if (leftHit.collider.gameObject.tag == "Room")
                {
                    // There's a room beside us
                    Instantiate(enemySpawnerObject, leftHit.point, Quaternion.identity);
                }
            }
        }

        enemySpawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawn");
    }

    void FindThisRoomsRoomTiles()
    {
        // we change the tags of the rooms, so we can get an array from them, then change the tags back
        if (topRight)
        {
            topRight.tag = "CombatRoom";
        }
        if (topLeft)
        {
            topLeft.tag = "CombatRoom";
        }
        if (bottomLeft)
        {
            bottomLeft.tag = "CombatRoom";
        }
        // if we use the game object here, it will use the center of the entire cluster of rooms, since this is the parent object.
        // so instead, we use a child object that is the same position we want, but have to make sure we don't ALSO include
        // the game object with the tag search, so it becomes untagged here
        parentRoomWorldCenterObject.tag = "CombatRoom";
        gameObject.tag = "Untagged";
    }

    void LoseThisRoomsRoomTiles()
    {
        // we change the tags of the rooms, so we can get an array from them, then change the tags back
        if (topRight)
        {
            topRight.tag = "Room";
        }
        if (topLeft)
        {
            topLeft.tag = "Room";
        }
        if (bottomLeft)
        {
            bottomLeft.tag = "Room";
        }
        parentRoomWorldCenterObject.tag = "Room";
        gameObject.tag = "Room";
    }
}
