using UnityEngine;
using Assets.Scripts;

public class Main : MonoBehaviour {

    public static int gameState = 0;
    public static float delay = 50f;
    int started = 0;

    public GameObject blob;
    public GameObject food;

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
        BlobManager.blobs.Clear();
        FoodManager.foods.Clear();

        var toClear = GameObject.FindGameObjectsWithTag("Blob");
        for (int i = 0; i < toClear.Length; i++) Destroy(toClear[i]);
        toClear = GameObject.FindGameObjectsWithTag("Food");
        for (int i = 0; i < toClear.Length; i++) Destroy(toClear[i]);
    }
	
	// Update is called once per frame
	void Update () {
        BlobManager.blobspeed = Main.delay;
        FoodManager.spawnspeed = Main.delay * FoodManager.foodSpawnRate;

	    if (gameState == 1)
        {
            if (started == 0)
            {
                started = 1;
                for (int i = 1; i <= 10; i++)
                {
                    GameObject clone = GameObject.Instantiate(blob, new Vector3(((float)Random.Range(-5000, 5000)) / 1000, ((float)Random.Range(-5000, 5000)) / 1000), new Quaternion(0, 0, 0, 0)) as GameObject;
                    BlobManager.blobs.Add(clone);
                }
            }
            FoodManager.Spawn(food);
        }
	}
}
