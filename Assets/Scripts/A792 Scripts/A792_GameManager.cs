using UnityEngine;
using System.Collections;

public class A792_GameManager : MonoBehaviour
{

    public static GameObject[] activeCombatRooms;

    public static int enemiesLeftInTheCombatRoom;
    public static bool isFightingInACombatRoom;

    public GameObject[] activeCombatRoomsVisual;
    [Header("Enemies")]
    public GameObject enemy_MrToots;
    public GameObject enemy_Guard;

    // Use this for initialization
    void Start ()
    {
        isFightingInACombatRoom = false;
        enemiesLeftInTheCombatRoom = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //activeCombatRoomsVisual = activeCombatRooms;
	}

    // this is called from our combat room script, it sets it up here, so enemies when spawned can find where to move to easily
    public static void FindActiveCombatRooms()
    {
        activeCombatRooms = GameObject.FindGameObjectsWithTag("CombatRoom");
    }

    public static void ClearActiveCombatRooms()
    {
        activeCombatRooms = null;
    }
}
