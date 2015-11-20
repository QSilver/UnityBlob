using UnityEngine;
using Assets.Scripts;

public class Blob : MonoBehaviour {

    private Rigidbody rb;
    GameObject target;
    public float speed;

    float energy = 100f;
    float range = 10f;
    float t;
    float delay = BlobManager.blobspeed;
    int foundFood = 0;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        rb.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        this.GetComponentInChildren<SpriteRenderer>().color = new Color(this.energy/150, 0, 0, 1);
        if (Main.gameState == 1)
        {
            delay = BlobManager.blobspeed;
            t += Time.deltaTime * 1000;
            if (t > delay)
            {
                DebugFind();
                t = 0f;
                energy -= 1;
            }
            if (this.energy > 200f)
            {
                Reproduce();
            }
            if (this.energy < 0f)
            {
                Starve();
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Food")
        {
            Eat();
            FoodManager.foods.Remove(other.gameObject);
            Destroy(other.gameObject);
            Destroy(other);
        }
    }

    void OnCollisonEnter(Collider other)
    {
        if (other.tag == "Blob")
        {
            this.energy -= 10;
        }
    }

    void Eat()
    {
        this.energy += 30;
    }

    void RandomMove()
    {
        float angle = Random.Range(0, 361) * Mathf.PI / 180;
        rb.transform.position += new Vector3(speed * Mathf.Cos(angle) / 10, speed * Mathf.Sin(angle) / 10);
    }

    void Reproduce()
    {
        GameObject child1 = GameObject.Instantiate(this, this.transform.position - new Vector3(-0.5f,-0.5f), this.transform.rotation) as GameObject;
        BlobManager.blobs.Add(child1);
        GameObject child2 = GameObject.Instantiate(this, this.transform.position + new Vector3(0.5f, 0.5f), this.transform.rotation) as GameObject;
        BlobManager.blobs.Add(child2);

        Starve();
    }

    void Starve()
    {
        BlobManager.blobs.Remove(this.gameObject);
        Destroy(this.gameObject);
        Destroy(this);
    }

    void DebugFind()
    {
        target = Find(float.MaxValue);
        if (foundFood == 1)
        {

            if (target != null)
            {
                float angle = Mathf.Atan2((this.transform.position.y - target.transform.position.y),(this.transform.position.x - target.transform.position.x)) - Mathf.PI;
                rb.transform.position += new Vector3(speed * Mathf.Cos(angle) / 10, speed * Mathf.Sin(angle) / 10);
            }
            else
            {
                foundFood = 0;
            }
        }
        else
        {
            RandomMove();
        }
    }

    GameObject Find(float min)
    {
        GameObject store = null;
        foreach (GameObject food in FoodManager.foods)
        {
            float dist = Vector3.Distance(this.transform.position, food.transform.position);
            if (dist < this.range)
            {
                if (dist < min)
                {
                    min = dist;
                    foundFood = 1;
                    store = food;
                }
            }
        }
        return store;
    }
}
