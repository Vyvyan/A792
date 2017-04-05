using UnityEngine;
using System.Collections;

public class MrToots : MonoBehaviour {

    // references to things we need to destroy on death/change
    public s_WanderingAI wanderingScript;
    public GameObject deadToots, aliveToots, siren, arm1, arm2;
    public NavMeshAgent agent;

    [Header("Health Stuff")]
    public int health;

    public bool isAlive;

    GameObject player;

    // Use this for initialization
    void Start ()
    {
        isAlive = true;
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (isAlive)
        {
            if (health <= 0)
            {
                Kill();
            }
        }

        // WE HUNT DA PLAYA
        if (agent)
        {
            agent.SetDestination(player.transform.position);
        }
	}

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            TakeDamage(2, other.gameObject.transform.position);
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
                Kill();
            }
        }
    }

    void Kill()
    {
        Destroy(aliveToots);
        Destroy(wanderingScript);
        Destroy(agent);
        deadToots.SetActive(true);
        arm1.AddComponent<Rigidbody>();
        arm2.AddComponent<Rigidbody>();
        siren.AddComponent<Rigidbody>();
        Destroy(gameObject.GetComponent<BoxCollider>());
        AutoDestroy autoDestroy = gameObject.transform.parent.gameObject.AddComponent<AutoDestroy>();
        autoDestroy.timeUntilDestroy = 8;
        isAlive = false;

        // explosion
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, 5);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(500, gameObject.transform.position, 5, .5f);
        }
    }
}
