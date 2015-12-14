using UnityEngine;
using System.Linq;
using Assets.Scripts;

public class Main : MonoBehaviour
{
    public static float timestamp = 0f;

    public static int gameState = 0;
    public static float delay = 50f;
    public static float hostility = 1f;
    public static bool hasSound;
    int started = 0;

    public GameObject blob;
    public GameObject food;
    public static System.Random random = new System.Random();

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    void Start()
    {
        Application.runInBackground = true;
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
        gameState = 0; started = 0;
        BlobManager.blobs.Clear();
        FoodManager.foods.Clear();

        timestamp = 0; BlobLogic.ID = 0; Log.Reset();

        var toClear = GameObject.FindGameObjectsWithTag("Blob");
        for (int i = 0; i < toClear.Length; i++) Destroy(toClear[i]);
        toClear = GameObject.FindGameObjectsWithTag("Food");
        for (int i = 0; i < toClear.Length; i++) Destroy(toClear[i]);
    }

	void Update ()
    {
        BlobManager.blobspeed = Main.delay;
        FoodManager.spawnspeed = Main.delay * FoodManager.foodSpawnRate;

	    if (gameState == 1)
        {
            timestamp += Time.deltaTime;
            if (started == 0)
            {
                started = 1;
                for (int i = 1; i <= 10; i++)
                {
                    GameObject clone = GameObject.Instantiate(blob, new Vector3(((float)Random.Range(-5000, 5000)) / 1000, ((float)Random.Range(-5000, 5000)) / 1000), new Quaternion(0, 0, 0, 0)) as GameObject;
                    clone.AddComponent<BlobDNA>();
                    clone.GetComponent<BlobDNA>().setDNA(DNAOperations.generate(DNAOperations.DNASIZE));
                    BlobManager.blobs.Add(clone);
                }
            }
            FoodManager.Spawn(food);
        }
	}
}
