using UnityEngine;
using System.Collections;

public class Guard : BadGuy
{
    Vector3 moveTarget;
    NavMeshAgent agent;

    Animator anim;
    Rigidbody rb;
    Rigidbody[] limbRBs;

    GameObject player; 
    public GameObject attackTarget;

    float attackTimer;

    public Transform visionObject;
    public GameObject gun;
    public GameObject bullet;
    public GameObject bulletSpawnPoint;

    Vector3 lastKnownPositionOfTarget;

    // clean up
    DestroyAfterTimePrompt bodyCleanUp;

    public bool canSeePlayer;

    bool isAlive;

    public enum AiState { movingToFirePoint, Attacking};
    public AiState aiState;

    // layer mask for finding enemies close-by (note, enemies meaning enemies of the bad guys... so players and friends)
    int layerMask = 1 << 10;


    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        aiState = AiState.movingToFirePoint;
        limbRBs = GetComponentsInChildren<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        bodyCleanUp = GetComponentInParent<DestroyAfterTimePrompt>();
        attackTimer = attackRate;
        isAlive = true;

        // Find our closest combat room, so we know where to run to (since the CR is made up of possibly 4 combat rooms, we dont want to run all the way across it)
        if (A792_GameManager.activeCombatRooms != null)
        {
            GameObject closestCR = FindClosestCombatRoom(A792_GameManager.activeCombatRooms);
            // now we have the closest room, now to find a random point in the room to run to
            Vector2 tempVec2 = new Vector2(closestCR.transform.position.x, closestCR.transform.position.z) + Random.insideUnitCircle * 22;
            moveTarget = new Vector3(tempVec2.x, 0, tempVec2.y);

            // now make them move there
            if (moveTarget != null)
            {
                agent.SetDestination(moveTarget);
            }
        }       
    }

    // Update is called once per frame
    void Update()
    {
        // AI STATE CHANGES
        if (aiState == AiState.movingToFirePoint)
        {
            // checks if we got to our destination
            float dist = agent.remainingDistance;
            if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
            { 
                Debug.Log("switched");
                aiState = AiState.Attacking;
            }
        }
        else if (aiState == AiState.Attacking)
        {
            // do we have a target?! Then shoot at that dummy, shoot him ded
            if (attackTarget)
            {
                // we reached out destination
                anim.SetBool("isAiming", true);
                anim.SetBool("isFiring", true);
                //face the player/target
                Vector3 targetWithoutY = new Vector3(attackTarget.transform.position.x, gameObject.transform.parent.transform.position.y, attackTarget.transform.position.z);
                gameObject.transform.parent.gameObject.transform.LookAt(targetWithoutY);
            }
            // we don't have a target, we should get one
            else
            {
                // change animation back to idle
                anim.SetBool("isAiming", false);
                anim.SetBool("isFiring", false);
                // SWAPLS this needs to be more indepth to target skeltins nearby too
                attackTarget = player;
            }
        }

        // Animations
        // are we moving?
        if (agent.velocity != Vector3.zero)
        {
            anim.SetFloat("MoveVelocity", 1);
            // this should help with the guards sometimes switching AI states before they should
            aiState = AiState.movingToFirePoint;
        }
        else
        {
            anim.SetFloat("MoveVelocity", 0);
        }
    }

    GameObject FindClosestCombatRoom(GameObject[] activeCrRooms)
    {
        if (activeCrRooms.Length > 1)
        {
            GameObject tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;
            foreach (GameObject t in activeCrRooms)
            {
                float dist = Vector3.Distance(t.transform.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
            return tMin;
        }
        else
        {
            return activeCrRooms[0];
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("something hit us");
        if (other.gameObject.tag == "Bullet")
        {
            TakeDamage(2, other.gameObject.transform.position);
            // now change the bullets tag, so it wont hurt anything else
            other.gameObject.tag = "Untagged";          
        }
    }

    public void TakeDamage(int damage, Vector3 hitLocation)
    {
        if (isAlive)
        {
            Debug.Log("we took " + damage.ToString() + " damage.");
            // remove health
            health -= damage;

            // if we have no health, kill us
            if (health <= 0)
            {
                KillThisGuard();
            }
        }
    }

    public void KillThisGuard()
    {
        isAlive = false;
        foreach (Rigidbody tempRig in limbRBs)
        {
            tempRig.velocity = Vector3.zero;
        }
        rb.velocity = Vector3.zero;
        rb.isKinematic = false;
        Destroy(anim);
        Destroy(agent);
        // make the gun physics, also parent the gun to the parent object, so it still gets destroyed, but doesn't move with the arm
        gun.transform.SetParent(gameObject.transform.parent);
        gun.AddComponent<Rigidbody>();
        bodyCleanUp.countingDown = true;
        // add humanity
        player.GetComponent<A792_Player>().humanity++;

        // if we're in a combat room, make sure our death is counted
        if (A792_GameManager.isFightingInACombatRoom)
        {
            A792_GameManager.enemiesLeftInTheCombatRoom--;
        }

        Destroy(this);
    }

    public void FireGun()
    {
        GameObject tempBullet = Instantiate(bullet, bulletSpawnPoint.transform.position, Quaternion.identity) as GameObject;
        Vector3 dir = ((attackTarget.transform.position - bulletSpawnPoint.transform.position).normalized);
        tempBullet.GetComponent<Rigidbody>().AddForce(dir * 35, ForceMode.VelocityChange);
    }

    void FindATarget()
    {
        
    }
}
