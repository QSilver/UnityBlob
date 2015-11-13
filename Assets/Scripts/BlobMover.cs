using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class BlobMover : MonoBehaviour {

    private Rigidbody rb;
    public float speed;

    float t;
    float delay = BlobManager.delay;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        speed = speed / 10;
	}
	
	// Update is called once per frame
	void Update () {
        if (Main.gameState == 1)
        {
            delay = BlobManager.delay;
            t += Time.deltaTime * 1000;
            if (t > delay)
            {
                blobMove();
                t = 0f;
            }
        }
	}

    void blobMove()
    {
        int dir = Random.Range(0, 4);
        switch (dir)
        {
            case 0: rb.transform.position += new Vector3(speed,0,0); break;
            case 1: rb.transform.position += new Vector3(-speed, 0, 0); break;
            case 2: rb.transform.position += new Vector3(0, speed, 0); break;
            case 3: rb.transform.position += new Vector3(0, -speed, 0); break;
        }
    }
}
