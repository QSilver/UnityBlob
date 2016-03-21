using UnityEngine;
using Assets.Scripts;
using System.Collections.Generic;

public class BlobLogic : MonoBehaviour
{
    private Rigidbody rb;
    private AudioSource audioSource;
    public AudioClip drop;
    public AudioClip chomp;
    public GameObject blob;
    GameObject target;
    public float speed;

    float energy = 100f;
    float range = 0.1f;
    float toReproduce = 200f;
    float t;
    float delay = BlobManager.blobspeed;
    float angle = 0f;
    int DNAaux;

    int colourfix = 50;
    int levytime = 120;
    int levythreshold = 1;
    bool isLevy = false;

    List<float> ate = new List<float>();

    public static int ID;
    protected int blobID;
    public int showID;

	void Start ()
    {
        // load Unity components
        rb = blob.GetComponent<Rigidbody>();
        audioSource = blob.GetComponent<AudioSource>();

        DNAaux = 0; // current position in DNA string
        // levytime = first DNALEVYTIME bits
        this.levytime = (System.Convert.ToInt32(blob.GetComponent<BlobDNA>().getDNA().Substring(DNAaux, DNAOperations.DNALEVYTIME), 2));
        DNAaux += DNAOperations.DNALEVYTIME;
        // toReproduce = next DNAREPROD bits
        this.toReproduce = ((float)System.Convert.ToInt32(blob.GetComponent<BlobDNA>().getDNA().Substring(DNAaux, DNAOperations.DNAREPROD), 2)) + 100f;
        DNAaux += DNAOperations.DNAREPROD;

        this.blobID = ID++;

        // switch audioclip
        audioSource.clip = drop;
        if (Main.hasSound)
        {
            audioSource.Play();
        }

        // make blob think it has eaten; otherwise it goes 
        for (int i = 0; i <= levythreshold; i++)
        {
            ate.Add(Main.globaltimestamp);
        }
	}
	
	void Update ()
    {

        showID = blobID;
        rb.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.transform.position = new Vector3(rb.transform.position.x, rb.transform.position.y, 0);

        this.GetComponentInChildren<SpriteRenderer>().color = new Color(0, 0, ((float)(levytime + colourfix)) / 255, 1);

        float blobsize = (this.energy + 50) / 150;
        this.gameObject.transform.localScale = new Vector3(blobsize, blobsize, 1);

        if (Main.gameState == Main.GameState.Started)
        {
            delay = BlobManager.blobspeed;
            t += Time.deltaTime * 1000;
            if (t > delay)
            {
                Move();
                t = 0f;
                energy -= Main.hostility;
            }
            if (this.energy > toReproduce)
            {
                Reproduce();
            }
            if (this.energy < 0f)
            {
                Starve();
            }
        }
	}

    void Move()
    {
        
        #region Turn Back
        float turn = FoodManager.foodSpawnSize / 2;
        if (rb.transform.position.x <= -turn || rb.transform.position.x >= turn || rb.transform.position.y <= -turn || rb.transform.position.y >= turn)
        {
            angle = angle - Mathf.PI - Mathf.PI * (float)(Main.random.NextDouble() - 0.5);
        }
        #endregion

        LevyUpdate();
        if (ate.Count < levythreshold)
        {
            if (isLevy == false) // is not already in Levy
            {
                // pick random direction
                angle = Random.Range(0, 361) * Mathf.PI / 180;
                isLevy = true;
            }
            // move the direction set by "angle"
            rb.transform.position += new Vector3(speed * Mathf.Cos(angle) / 10, speed * Mathf.Sin(angle) / 10);
        }
        else
        {
            // should not levy
            isLevy = false;
            RandomMove();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Food")
        {
            Eat();
            Destroy(other.gameObject);
            FoodManager.foods.Remove(other.gameObject);
            Destroy(other);
        }
    }

    void OnMouseDown()
    {
        BlobDisplay.Load(this);
    }

    void Eat()
    {
        audioSource.clip = chomp;
        if (Main.hasSound) audioSource.Play();
        this.energy += FoodManager.foodEnergy;
        ate.Add(Main.globaltimestamp);
    }

    void RandomMove()
    {
        float angle = Random.Range(0, 361) * Mathf.PI / 180;
        rb.transform.position += new Vector3(speed * Mathf.Cos(angle) / 10, speed * Mathf.Sin(angle) / 10);
    }

    void Reproduce()
    {
        GameObject child1 = GameObject.Instantiate(blob, blob.transform.position, blob.transform.rotation) as GameObject;
        child1.AddComponent<BlobDNA>();
        child1.GetComponent<BlobDNA>().setDNA(DNAOperations.mutate(blob.GetComponent<BlobDNA>().getDNA()));
        child1.GetComponent<BlobLogic>().energy = this.energy / 2;
        BlobManager.blobs.Add(child1);

        GameObject child2 = GameObject.Instantiate(blob, blob.transform.position, blob.transform.rotation) as GameObject;
        child2.AddComponent<BlobDNA>();
        child2.GetComponent<BlobDNA>().setDNA(DNAOperations.mutate(blob.GetComponent<BlobDNA>().getDNA()));
        child2.GetComponent<BlobLogic>().energy = this.energy / 2;
        BlobManager.blobs.Add(child2);

        Destroy(blob.gameObject);
        BlobManager.blobs.Remove(blob.gameObject);
        Destroy(blob);
    }

    void Starve()
    {
        Destroy(this.gameObject);
        BlobManager.blobs.Remove(this.gameObject);
        Destroy(this);
    }

    void LevyUpdate()
    {
        // update blob "memory" list
        for (int i = 0; i < ate.Count; i++)
            if (ate[i] < Main.globaltimestamp - levytime)
                ate.Remove(ate[i]);
    }

    public float getEnergy()
    {
        return this.energy;
    }

    public int getID()
    {
        return blob.GetComponent<BlobLogic>().blobID;
    }

    public float getRange()
    {
        return blob.GetComponent<BlobLogic>().range;
    }

    public float getReprod()
    {
        return this.toReproduce;
    }

    public float getPatience()
    {
        return this.levytime;
    }

    public float getAngle()
    {
        return this.angle;
    }

    public void set(int id, float energy, float x, float y, float angle)
    {
        rb = blob.GetComponent<Rigidbody>();
        rb.transform.position = new Vector3(x, y, 0);
        this.energy = energy;
        this.blobID = id;
        this.angle = angle;
    }
}
