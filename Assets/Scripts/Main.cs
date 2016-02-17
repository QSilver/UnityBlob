using UnityEngine;
using System.Linq;
using Assets.Scripts;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public WMG_Series population_data;
    public WMG_Series population_reprod;
    public WMG_Series population_patience;

    public static float timestamp = 0f;

    public static int gameState = 0;
    public static float delay = 50f;
    public static float hostility = 0.1f;
    public static bool hasSound;
    public static float globaltimestamp = 0f;

    int started = 0;
    int linetick = 0, lineupdate = 30;

    public GameObject blob;
    public GameObject food;
    public GameObject gamearea;
    public InputField inputfield;
    public static System.Random random = new System.Random();

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    void Start()
    {
        Application.runInBackground = true;
        float bgScale = 0.049f;
        gamearea.transform.localScale = new Vector3(FoodManager.foodSpawnSize * bgScale, FoodManager.foodSpawnSize * bgScale, 1);
    }

    /* void OnGUI()
    {
        Texture2D water = new Texture2D(1, 1);
        water.SetPixel(0, 0, new Color(0, 128, 192, 255));
        water.Apply();

        GUI.Box(new Rect(-1 * FoodManager.foodSpawnSize / 2, -1 * FoodManager.foodSpawnSize / 2, FoodManager.foodSpawnSize, FoodManager.foodSpawnSize), GUIContent.none);
    } */

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
        ResetLineGraph();
        BlobManager.blobs.Clear();
        FoodManager.foods.Clear();

        timestamp = 0; BlobLogic.ID = 0;

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
            linetick++; if (linetick >= lineupdate)
            {
                linetick = 0;
                UpdateLineGraph();
            }

            globaltimestamp += 1;

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

                FoodManager.Cluster();
            }
            FoodManager.Spawn(food);
        }
	}

    public void UpdateLineGraph()
    {
        population_data.pointValues.Add(new Vector2(0, BlobManager.blobs.Count));
        population_data.pointValues.Remove(population_data.pointValues[0]);

        float avgreprod = 0f;
        foreach (GameObject b in BlobManager.blobs)
            avgreprod += b.GetComponent<BlobLogic>().getReprod();
        if (BlobManager.blobs.Count != 0) avgreprod /= BlobManager.blobs.Count;
        else avgreprod = 0;
        population_reprod.pointValues.Add(new Vector2(0, avgreprod));
        population_reprod.pointValues.Remove(population_reprod.pointValues[0]);

        float avgpati = 0f;
        foreach (GameObject b in BlobManager.blobs)
            avgpati += b.GetComponent<BlobLogic>().getPatience();
        if (BlobManager.blobs.Count != 0) avgpati /= BlobManager.blobs.Count;
        else avgpati = 0;
        population_patience.pointValues.Add(new Vector2(0, avgpati));
        population_patience.pointValues.Remove(population_patience.pointValues[0]);
    }

    public void ResetLineGraph()
    {
        population_data.pointValues.Clear();
        for (int i = 0; i < 100; i++)
            population_data.pointValues.Add(new Vector2(0, 0));

        population_reprod.pointValues.Clear();
        for (int i = 0; i < 100; i++)
            population_reprod.pointValues.Add(new Vector2(0, 0));

        population_patience.pointValues.Clear();
        for (int i = 0; i < 100; i++)
            population_patience.pointValues.Add(new Vector2(0, 0));
    }

    public void SaveState()
    {
        string filename = inputfield.text;

        System.IO.StreamWriter file1 = new System.IO.StreamWriter(filename + "_blobs.txt");
        foreach (GameObject blob in BlobManager.blobs)
        {
            file1.WriteLine(blob.GetComponent<BlobLogic>().getID() + " " +
                            blob.GetComponent<BlobLogic>().getEnergy() + " " +
                            blob.transform.position.x + " " + blob.transform.position.y + " " +
                            blob.GetComponent<BlobLogic>().getAngle() + " " +
                            blob.GetComponent<BlobDNA>().getDNA());
        }
        file1.Close();

        System.IO.StreamWriter file2 = new System.IO.StreamWriter(filename + "_food.txt");
        foreach (GameObject f in FoodManager.foods)
        {
            file2.WriteLine(f.transform.position.x + " " + f.transform.position.y);
        }
        file2.Close();
        inputfield.text = "";
    }

    public void LoadState()
    {
        string filename = inputfield.text;

        System.IO.StreamReader file2 = new System.IO.StreamReader(filename + "_food.txt");
        string textline = "";

        do
        {
            textline = file2.ReadLine();
            string[] linesplit = textline.Split(' ');
            FoodManager.Place(food, float.Parse(linesplit[0]), float.Parse(linesplit[1]));
        } while (file2.Peek() != -1);

        file2.Close();
        inputfield.text = "";
    }
}
