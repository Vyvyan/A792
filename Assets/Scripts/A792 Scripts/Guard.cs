using UnityEngine;
using System.Collections;

public class Guard : BadGuy
{

    public float wanderRadius;
    public float wanderTimer;
    public bool randomWanderTime;

    private Transform moveTarget;
    private NavMeshAgent agent;
    private float timer;

    Animator anim;
    Rigidbody rb;
    Rigidbody[] limbRBs;

    GameObject player; 
    public GameObject attackTarget;

    float attackTimer;

    public Transform visionObject;
    public GameObject gun;

    Vector3 lastKnownPositionOfTarget;

    // clean up
    DestroyAfterTimePrompt bodyCleanUp;

    public bool canSeePlayer;

    bool isAlive;

    public enum AiState { wandering, attacking, outOfRange };
    public AiState aiState;

    // layer mask for finding enemies close-by (note, enemies meaning enemies of the bad guys... so players and friends)
    int layerMask = 1 << 10;

    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        timer = wanderTimer;
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        aiState = AiState.wandering;
        if (randomWanderTime)
        {
            wanderTimer = Random.Range(2, 15);
        }
        limbRBs = GetComponentsInChildren<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        bodyCleanUp = GetComponentInParent<DestroyAfterTimePrompt>();
        attackTimer = attackRate;
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        // These rays are ONLY to see if we can still see our target, NOT to find a new target. That is in a different function below, and those are local ray variables
        Ray visRay;
        RaycastHit visRayHit;

        // AI
        // if we're wandering
        if (aiState == AiState.wandering)
        {
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                if (randomWanderTime)
                {
                    wanderTimer = Random.Range(2, 15);
                }
                timer = 0;
            }

        }
        else if (aiState == AiState.attacking)
        {
            // we check this again here, for the frame the object gets destroyed
            if (attackTarget)
            {
                // face the player/target
                Vector3 targetWithoutY = new Vector3(attackTarget.transform.position.x, gameObject.transform.parent.transform.position.y, attackTarget.transform.position.z);
                gameObject.transform.parent.gameObject.transform.LookAt(targetWithoutY);
                // animation
                anim.SetBool("isAiming", true);

                // attack timer, then attack
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                {
                    // attack!
                    anim.SetBool("isFiring", true);
                }

                // while we're attacking, if we can't see out target anymore, then we should try to find him again
                visionObject.LookAt(attackTarget.transform.position);
                visRay = new Ray(visionObject.transform.position, visionObject.transform.forward);
                if (Physics.Raycast(visRay, out visRayHit, attackRange))
                {
                    if (visRayHit.collider.gameObject == attackTarget)
                    {
                        // yay! we can still see our target, no problems here.
                    }
                    else
                    {
                        // we can't see the target anymore
                        lastKnownPositionOfTarget = attackTarget.transform.position;
                        anim.SetBool("isAiming", false);
                        anim.SetBool("isFiring", false);
                        agent.SetDestination(lastKnownPositionOfTarget);
                        attackTarget = null;
                        aiState = AiState.outOfRange;
                    }
                }
            }
        }
        else if (aiState == AiState.outOfRange)
        {
            // once we go to the last known of the target, we can go back to wandering. If we see the target again, we'll switch back to attacking, because of the code below
            if (agent.hasPath && agent.remainingDistance <= 1f)
            {
                aiState = AiState.wandering;
            }
        }

        // IF WE AREN'T ATTACKING, WE CAN FIND A TARGET
        if (aiState != AiState.attacking)
        {
            // if we see the player to attack, then switch to attacking
            if (attackTarget)
            {
                // set our destination to where we're standing (round about way of stopping us, idk this sounded better for some reason)
                agent.SetDestination(gameObject.transform.position);
                // change ai state
                attackTimer = attackRate;
                aiState = AiState.attacking;
            }
        }

        // if we aren't searching for our target that we lost, then we go back to wandering if we don't have a target
        if (aiState != AiState.outOfRange)
        {
            // if we ever don't have a target, then go back to wandering
            if (!attackTarget || attackTarget == null)
            {
                aiState = AiState.wandering;
                anim.SetBool("isAiming", false);
                anim.SetBool("isFiring", false);
                anim.SetBool("isThrowing", false);
            }
        }

        // sets the float for animation movement based on the velocity of our agent
        float mag = agent.velocity.magnitude;
        anim.SetFloat("MoveVelocity", mag);
    }

    void FixedUpdate()
    {
        // vision
        // if we don't have an attack target
        if (!attackTarget)
        {
            // find things within range around us, but only if they are on the player and friends collision layer
            Collider[] nearbyThingsToShoot = Physics.OverlapSphere(gameObject.transform.position, attackRange / 2, layerMask);
            // now to find the first one that is in front of us, so we can target it.
            foreach(Collider col in nearbyThingsToShoot)
            {
                Debug.Log(col.gameObject.name.ToString() + "is in our physics overlay");
                visionObject.LookAt(col.transform.position);
                Ray ray = new Ray(visionObject.transform.position, visionObject.transform.forward);
                RaycastHit hit;
                if (Physics.SphereCast(ray, 1f, out hit, 100))
                {
                    // did we hit the object we're looking for?
                    if (hit.collider.transform.parent == col.transform.parent)
                    {
                        attackTarget = col.gameObject;
                        Debug.Log("Our target is: " + attackTarget.name.ToString());
                        break;
                    }
                }
            }
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
        Destroy(this);
    }

    public void FireGun()
    {

    }
}
