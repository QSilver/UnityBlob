using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

    public static int gameState = 0;
    int started = 0;

    public GameObject blob;
    List<GameObject> blobs = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
    public void StartPressed()
    {
        gameState = 1;
        Update();
    }

    public void PausePressed()
    {
        gameState = 0;
    }

    public void ResetPressed()
    {
        gameState = 0;
        started = 0;
        blobs.Clear();
        var toClear = GameObject.FindGameObjectsWithTag("Blob");
        for (int i = 0; i < toClear.Length; i++) Destroy(toClear[i]);
    }
	
	// Update is called once per frame
	void Update () {
	    if (gameState == 1)
        {
            if (started == 0)
            {
                started = 1;
                for (int i = 1; i <= 10; i++)
                {
                    GameObject clone = Instantiate(blob, new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0), new Quaternion(0, 0, 0, 0)) as GameObject;
                    blobs.Add(clone);
                }
            }
        }
	}
}
