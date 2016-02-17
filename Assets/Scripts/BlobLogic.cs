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
    int foundFood = 0;
    int DNAaux;
    int state = 0;

    int levytime = 120;
    int levythreshold = 1;
    int shouldLevy = 0;
    int isLevy = 0;
    public int anglefix = 0;

    //int isr = 0, isg = 0, isb = 0;

    List<float> ate = new List<float>();

    public static int ID;
    protected int blobID;
    public int showID;

	// Use this for initialization
	void Start ()
    {
        rb = blob.GetComponent<Rigidbody>();
        audioSource = blob.GetComponent<AudioSource>();

        DNAaux = 0;
        this.levytime = (System.Convert.ToInt32(blob.GetComponent<BlobDNA>().getDNA().Substring(DNAaux, DNAOperations.DNALEVYTIME), 2));
        DNAaux += DNAOperations.DNALEVYTIME;
        this.toReproduce = ((float)System.Convert.ToInt32(blob.GetComponent<BlobDNA>().getDNA().Substring(DNAaux, DNAOperations.DNAREPROD), 2)) + 100f;
        DNAaux += DNAOperations.DNAREPROD;
        /*
        this.isr = (System.Convert.ToInt32(blob.GetComponent<BlobDNA>().getDNA().Substring(DNAaux, 1), 2));
        DNAaux += 1;
        this.isg = (System.Convert.ToInt32(blob.GetComponent<BlobDNA>().getDNA().Substring(DNAaux, 1), 2));
        DNAaux += 1;
        this.isb = (System.Convert.ToInt32(blob.GetComponent<BlobDNA>().getDNA().Substring(DNAaux, 1), 2));
        DNAaux += 1;
         */

        this.blobID = ID++;

        audioSource.clip = drop;
        if (Main.hasSound) audioSource.Play();

        for (int i = 0; i <= levythreshold; i++)
        {
            ate.Add(Main.globaltimestamp);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {

        showID = blobID;
        rb.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.transform.position = new Vector3(rb.transform.position.x, rb.transform.position.y, 0);

        this.GetComponentInChildren<SpriteRenderer>().color = new Color(0, 0, ((float)(levytime + 50)) / 256, 1);

        float blobsize = (this.energy + 50) / 150;
        //this.GetComponent<SphereCollider>().radius = (float)(0.1 * blobsize);
        this.gameObject.transform.localScale = new Vector3(blobsize, blobsize, 1);

        if (Main.gameState == 1)
        {
            delay = BlobManager.blobspeed;
            t += Time.deltaTime * 1000;
            if (t > delay)
            {
                Move();
                t = 0f;
                energy -= Main.hostility;
            }
            if (this.energy > toReproduce)// && BlobManager.blobs.Count < 1000)
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
        if (rb.transform.position.x <= -turn || rb.transform.position.x >= turn || rb.transform.position.y <= -turn || rb.transform.position.y >= turn) anglefix = 1;
        else anglefix = 0;

        if (anglefix == 1) angle = angle - Mathf.PI - Mathf.PI * (float)(Main.random.NextDouble() - 0.5);
        #endregion
        
        /*
        #region Wrap-Around
        float turn = FoodManager.foodSpawnSize / 2;
        if (rb.transform.position.x <= -turn) rb.transform.position = new Vector3(turn-1, rb.transform.position.y);
        if (rb.transform.position.x >= turn) rb.transform.position = new Vector3(-turn+1, rb.transform.position.y);
        if (rb.transform.position.y <= -turn) rb.transform.position = new Vector3(rb.transform.position.x, turn-1);
        if (rb.transform.position.y >= turn) rb.transform.position = new Vector3(rb.transform.position.x, -turn+1);
        #endregion
        */
        LevyUpdate();

        if (ate.Count < levythreshold) shouldLevy = 1;
        else shouldLevy = 0;
        if (foundFood == 1) shouldLevy = 0;

        if (shouldLevy == 1)
        {
            if (isLevy == 0)
            {
                angle = Random.Range(0, 361) * Mathf.PI / 180;
                isLevy = 1;
            }
            rb.transform.position += new Vector3(speed * Mathf.Cos(angle) / 10, speed * Mathf.Sin(angle) / 10);
            Find(float.MaxValue);
        }
        else
        {
            isLevy = 0;
            target = Find(float.MaxValue);
            if (foundFood == 1)
            {

                if (target != null)
                {
                    shouldLevy = 0;
                    angle = Mathf.Atan2((this.transform.position.y - target.transform.position.y), (this.transform.position.x - target.transform.position.x)) - Mathf.PI;
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

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Blob")
        {
            // this.energy -= 2; // fighting
        }
    }

    void OnMouseDown()
    {
        //Log.PassString((this.blobID + " Clicked"));
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
        this.state = 1;
        float angle = Random.Range(0, 361) * Mathf.PI / 180;
        rb.transform.position += new Vector3(speed * Mathf.Cos(angle) / 10, speed * Mathf.Sin(angle) / 10);
    }

    void Reproduce()
    {
        this.state = 2;
        GameObject child1 = GameObject.Instantiate(blob, blob.transform.position - new Vector3(0f, 0f), blob.transform.rotation) as GameObject;
        child1.AddComponent<BlobDNA>();
        child1.GetComponent<BlobDNA>().setDNA(DNAOperations.mutate(blob.GetComponent<BlobDNA>().getDNA()));
        child1.GetComponent<BlobLogic>().energy = this.energy / 2;
        BlobManager.blobs.Add(child1);

        GameObject child2 = GameObject.Instantiate(blob, blob.transform.position + new Vector3(0f, 0f), blob.transform.rotation) as GameObject;
        child2.AddComponent<BlobDNA>();
        child2.GetComponent<BlobDNA>().setDNA(DNAOperations.mutate(blob.GetComponent<BlobDNA>().getDNA()));
        child2.GetComponent<BlobLogic>().energy = this.energy / 2;
        BlobManager.blobs.Add(child2);

        //Log.PassString(("<" + blob.GetComponent<BlobDNA>().getDNA() + ">" + " Reproduced " + child1.GetComponent<BlobDNA>().getDNA() + " " + child2.GetComponent<BlobDNA>().getDNA()));

        // duplicate to avoid logging death
        Destroy(blob.gameObject);
        BlobManager.blobs.Remove(blob.gameObject);
        Destroy(blob);
    }

    void Starve()
    {
        //Log.PassString(("<" + blob.GetComponent<BlobDNA>().getDNA() + ">" + " Died"));

        Destroy(this.gameObject);
        BlobManager.blobs.Remove(this.gameObject);
        Destroy(this);
    }

    void LevyUpdate()
    {
        for (int i = 0; i < ate.Count; i++)
            if (ate[i] < Main.globaltimestamp - levytime)
                ate.Remove(ate[i]);
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

    public int getState()
    {
        return this.state;
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
}
