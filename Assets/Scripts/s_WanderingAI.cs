using UnityEngine;
using System.Collections;

public class s_WanderingAI : MonoBehaviour
{

    public float wanderRadius;
    public float wanderTimer;

    private Transform target;
    private UnityEngine.AI.NavMeshAgent agent;
    private float timer;
    public bool wander;

    GameObject player;

    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponentInParent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        timer = wanderTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (wander)
        {
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }

            // this is for changing wander if the player is close enough to the enemy
            if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 50)
            {
                wander = false;
            }
        }
        else
        {
            if (player)
            {
                agent.SetDestination(player.transform.position);
            }
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        UnityEngine.AI.NavMeshHit navHit;

        UnityEngine.AI.NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}