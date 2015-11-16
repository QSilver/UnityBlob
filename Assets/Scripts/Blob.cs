using UnityEngine;
using Assets.Scripts;

public class Blob : MonoBehaviour {

    private Rigidbody rb;
    public float speed;

    float energy = 100f;
    float t;
    float delay = BlobManager.blobspeed;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Main.gameState == 1)
        {
            delay = BlobManager.blobspeed;
            t += Time.deltaTime * 1000;
            if (t > delay)
            {
                Move();
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

    void Eat()
    {
        this.energy += 300;
    }

    void Move()
    {
        int dir = Random.Range(0, 4);
        switch (dir)
        {
            case 0: rb.transform.position += new Vector3(speed/10,0,0); break;
            case 1: rb.transform.position += new Vector3(-speed/10, 0, 0); break;
            case 2: rb.transform.position += new Vector3(0, speed/10, 0); break;
            case 3: rb.transform.position += new Vector3(0, -speed/10, 0); break;
        }
    }

    void Reproduce()
    {
        GameObject child1 = GameObject.Instantiate(this, this.transform.position, this.transform.rotation) as GameObject;
        BlobManager.blobs.Add(child1);
        GameObject child2 = GameObject.Instantiate(this, this.transform.position, this.transform.rotation) as GameObject;
        BlobManager.blobs.Add(child2);

        Starve();
    }

    void Starve()
    {
        BlobManager.blobs.Remove(this.gameObject);
        Destroy(this.gameObject);
        Destroy(this);
    }
}
