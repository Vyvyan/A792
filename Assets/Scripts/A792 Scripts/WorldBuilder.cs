﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WorldBuilder : MonoBehaviour {

    public int roomsPerPass, numberOfPasses;
    int roomsPerPassStarting;
    public static bool isWorldBuilt;
    [Header("Normal Rooms")]
    public GameObject[] rooms;
    [Header("Combat Rooms")]
    public GameObject[] combatRooms;
    bool hasFinishedBuildingWorld;
    // this decides if we want to have a higher chance of moving a certain direction each pass
    int directionBias;
    // decides where in this queue we spawn the combat room
    int combatRoomQueueNumber;

    // starting position
    Vector3 startingPosition;

    // Use this for initialization
    void Start ()
    {
        isWorldBuilt = false;
        hasFinishedBuildingWorld = false;
        roomsPerPassStarting = roomsPerPass;
        BiasMovement();
        startingPosition = gameObject.transform.position;
    }

    void Update()
    {
        
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Ray roomRay = new Ray(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 25, gameObject.transform.position.z), Vector3.down);
        RaycastHit roomRayHit;

        if (numberOfPasses > 0)
        {
            if (roomsPerPass > 0)
            {
                if (Physics.Raycast(roomRay, out roomRayHit))
                {
                    if (roomRayHit.collider.gameObject.tag == "Ground")
                    {
                        // There's not a room here! Spawn one!
                        // random a room to spawn
                        int rnd = Random.Range(0, rooms.Length);
                        // spawn it
                        if (roomsPerPass == combatRoomQueueNumber)
                        {
                            // Time to spawn a combat room!
                            int crRND = Random.Range(0, combatRooms.Length);
                            Instantiate(combatRooms[crRND], gameObject.transform.position, Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(rooms[rnd], gameObject.transform.position, Quaternion.identity);
                        }
                        // decrease our room number to spawn
                        roomsPerPass--;
                    }
                }

                // move our spawner
                int rndPlaceToMove = Random.Range(1, 5);


                if (rndPlaceToMove == 1)
                {
                    //up
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 50);
                }
                else if (rndPlaceToMove == 2)
                {
                    // right
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x + 50, gameObject.transform.position.y, gameObject.transform.position.z);
                }
                else if (rndPlaceToMove == 3)
                {
                    // left
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x - 50, gameObject.transform.position.y, gameObject.transform.position.z);
                }
                else if (rndPlaceToMove == 4)
                {
                    // THIS IS OUR BIAS
                    if(directionBias == 1)
                    {
                        //up
                        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 50);
                    }
                    else if (directionBias == 2)
                    {
                        // right
                        gameObject.transform.position = new Vector3(gameObject.transform.position.x + 50, gameObject.transform.position.y, gameObject.transform.position.z);
                    }
                    else if(directionBias == 3)
                    {
                        // left
                        gameObject.transform.position = new Vector3(gameObject.transform.position.x - 50, gameObject.transform.position.y, gameObject.transform.position.z);
                    }
                }
            }
            else
            {
                // we have no more rooms this pass, so reset it
                gameObject.transform.position = startingPosition;
                numberOfPasses--;
                BiasMovement();
                roomsPerPass = roomsPerPassStarting;
            }
        }
        else
        {
            if (!hasFinishedBuildingWorld)
            {
                // now change all room exits based on the layout of the world
                Room[] rooms = FindObjectsOfType<Room>();
                foreach (Room spawnedRoom in rooms)
                {
                    spawnedRoom.CheckNeighbors();
                }
                CombatRoom[] combatRooms = FindObjectsOfType<CombatRoom>();
                foreach (CombatRoom cbRoom in combatRooms)
                {
                    cbRoom.ConfigureDoors();
                }
                isWorldBuilt = true;
                hasFinishedBuildingWorld = true;
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
	}

    void BiasMovement()
    {
        directionBias = Random.Range(1, 4);

        // we also decide where our combat rooms will spawn in this pass here
        combatRoomQueueNumber = Random.Range(1, roomsPerPassStarting - 3);
    }
}
