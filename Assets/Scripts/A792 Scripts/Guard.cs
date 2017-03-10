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

    public Transform visionObject;

    public bool canSeePlayer;

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
    }

    // Update is called once per frame
    void Update()
    {
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

            // if we see the player while wandering, then we should attack
            if (attackTarget)
            {
                // set our destination to where we're standing (round about way of stopping us, idk this sounded better for some reason)
                agent.SetDestination(gameObject.transform.position);
                // change ai state
                aiState = AiState.attacking;
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
            }
        }
        else if (aiState == AiState.outOfRange)
        {

        }

        // if we ever don't have a target, then go back to wandering
        if (!attackTarget || attackTarget == null)
        {
            aiState = AiState.wandering;
            anim.SetBool("isAiming", false);
            anim.SetBool("isFiring", false);
            anim.SetBool("isThrowing", false);
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
                if (Physics.Raycast(ray, out hit, 100))
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
}
