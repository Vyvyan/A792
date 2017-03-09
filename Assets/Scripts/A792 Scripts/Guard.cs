using UnityEngine;
using System.Collections;

public class Guard : BadGuy
{

    public float wanderRadius;
    public float wanderTimer;
    public bool randomWanderTime;

    private Transform target;
    private NavMeshAgent agent;
    private float timer;

    Animator anim;
    Rigidbody rb;
    Rigidbody[] limbRBs;

    GameObject player;

    public Transform visionObject;

    public bool canSeePlayer;

    public enum AiState { wandering, attacking, outOfRange };
    public AiState aiState;

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
            /*
            if (canSeePlayer)
            {
                aiState = AiState.attacking;
            }
            */
        }
        else if (aiState == AiState.attacking)
        {

        }
        else if (aiState == AiState.outOfRange)
        {

        }

        // sets the float for animation movement based on the velocity of our agent
        float mag = agent.velocity.magnitude;
        anim.SetFloat("MoveVelocity", mag);
    }

    void FixedUpdate()
    {
        // vision
        visionObject.LookAt(player.transform.position);
        Ray ray = new Ray(visionObject.transform.position, visionObject.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, visionRange))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                canSeePlayer = true;
            }
            else
            {
                canSeePlayer = false;
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
