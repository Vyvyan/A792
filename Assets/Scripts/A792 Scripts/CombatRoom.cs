using UnityEngine;
using System.Collections;

public class CombatRoom : MonoBehaviour {

    public bool isInCombat;
    bool hasInitiatedCombat;

    public CombatRoomDoorway[] doors;

    public GameObject enemySpawnerObject;

    [Header("Room Tiles")]
    // we don't need bottom right since that is this game object
    public GameObject topRight, topLeft, bottomLeft; 

    [Header("Enemies")]
    public GameObject mrToots_enemy;
    public int enemiesToSpawn;
    GameObject[] enemySpawnPoints;
    public float spawnRate, spawnRateCurrent;

	// Use this for initialization
	void Start ()
    {
        isInCombat = false;
        hasInitiatedCombat = false;
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
                    // instantiate our enemy (needs to change so we can spawn multiple types of enemies)
                    Instantiate(mrToots_enemy, enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)].transform.position, Quaternion.identity);
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
                // we killed all the enemies
                // ADD ANOTHER CHECK CAUSE WE NEED TO END COMBAT ONCE EVERYTHING IS DEAD NOT JST SPAWNED
                isInCombat = false;
                // destroy our spawn points
                foreach(GameObject spawner in enemySpawnPoints)
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
    }

    public void UnlockRoom()
    {
        foreach (CombatRoomDoorway doorway in doors)
        {
            doorway.TurnOffForceField();
        }
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
}
