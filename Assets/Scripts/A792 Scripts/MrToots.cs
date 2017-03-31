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

    // Use this for initialization
    void Start ()
    {
        isAlive = true;
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
    }
}
