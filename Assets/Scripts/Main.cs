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

    public enum GameState
    {
        Started,
        Paused
    }; 
    public static GameState gameState = GameState.Paused;

    public static int graphPoints = 200;
    public static float delay = 50f;
    public static float hostility = 0.1f;
    public static bool hasSound;
    public static float globaltimestamp = 0f;

    bool started = false;
    int linetick = 0, lineupdate = 30;

    public GameObject blob;
    public GameObject food;
    public GameObject gamearea;
    public InputField inputfield;

    private const float bgScale = 0.049f; // needed to transform between floating point space to screen coordinates

    private static System.Random seedGen = new System.Random();
    private static int seed = seedGen.Next();
    public static System.Random random = new System.Random(seed);

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    void Start()
    {
        Application.runInBackground = true;
        gamearea.transform.localScale = new Vector3(FoodManager.foodSpawnSize * bgScale, FoodManager.foodSpawnSize * bgScale, 1);
        ResetLineGraph();
    }

    public void StartPressed()
    {
        gameState = GameState.Started;
    }

    public void PausePressed()
    {
        gameState = GameState.Paused;
    }

    public void ResetPressed()
    {
        gameState = GameState.Paused;
        started = false;
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

	    if (gameState == GameState.Started)
        {
            linetick++;
            if (linetick >= lineupdate)
            {
                linetick = 0;
                UpdateLineGraph();
            }

            globaltimestamp += 1;

            timestamp += Time.deltaTime;
            if (started == false)
            {
                started = true;
                if (BlobManager.blobs.Count == 0)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        GameObject clone = GameObject.Instantiate(blob, new Vector3(((float)Random.Range(-5000, 5000)) / 1000, ((float)Random.Range(-5000, 5000)) / 1000), new Quaternion(0, 0, 0, 0)) as GameObject;
                        clone.AddComponent<BlobDNA>();
                        clone.GetComponent<BlobDNA>().setDNA(DNAOperations.generate(DNAOperations.DNASIZE));
                        BlobManager.blobs.Add(clone);
                    }
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
            if (b != null)
                avgreprod += b.GetComponent<BlobLogic>().getReprod();
        if (BlobManager.blobs.Count != 0) avgreprod /= BlobManager.blobs.Count;
        else avgreprod = 0;
        population_reprod.pointValues.Add(new Vector2(0, avgreprod));
        population_reprod.pointValues.Remove(population_reprod.pointValues[0]);

        float avgpati = 0f;
        foreach (GameObject b in BlobManager.blobs)
            if (b != null)
                avgpati += b.GetComponent<BlobLogic>().getPatience();
        if (BlobManager.blobs.Count != 0) avgpati /= BlobManager.blobs.Count;
        else avgpati = 0;
        population_patience.pointValues.Add(new Vector2(0, avgpati));
        population_patience.pointValues.Remove(population_patience.pointValues[0]);
    }

    public void ResetLineGraph()
    {
        population_data.pointValues.Clear();
        for (int i = 0; i < graphPoints; i++)
            population_data.pointValues.Add(new Vector2(0, 0));

        population_reprod.pointValues.Clear();
        for (int i = 0; i < graphPoints; i++)
            population_reprod.pointValues.Add(new Vector2(0, 0));

        population_patience.pointValues.Clear();
        for (int i = 0; i < graphPoints; i++)
            population_patience.pointValues.Add(new Vector2(0, 0));
    }

    public void SaveState()
    {
        string filename = inputfield.text;

        System.IO.StreamWriter seedfile = new System.IO.StreamWriter(filename + "_seed.txt");
        seedfile.WriteLine(seed);
        seedfile.Close();

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

        JSONExport(filename);
    }

    public void LoadState()
    {
        string filename = inputfield.text;

        System.IO.StreamReader seedfile = new System.IO.StreamReader(filename + "_seed.txt");
        int seed = int.Parse(seedfile.ReadLine());
        random = new System.Random(seed);
        seedfile.Close();

        System.IO.StreamReader file1 = new System.IO.StreamReader(filename + "_blobs.txt");
        string textline = "";

        do
        {
            textline = file1.ReadLine();
            string[] linesplit;
            try
            {
                linesplit = textline.Split(' ');
                BlobManager.Place(blob, int.Parse(linesplit[0]), float.Parse(linesplit[1]), float.Parse(linesplit[2]), float.Parse(linesplit[3]), float.Parse(linesplit[4]), linesplit[5]);
            }
            catch (System.NullReferenceException e) {}
        } while (file1.Peek() != -1);
        file1.Close();

        System.IO.StreamReader file2 = new System.IO.StreamReader(filename + "_food.txt");
        textline = "";

        do
        {
            textline = file2.ReadLine();
            string[] linesplit;
            try
            {
                linesplit = textline.Split(' ');
                FoodManager.Place(food, float.Parse(linesplit[0]), float.Parse(linesplit[1]));
            }
            catch (System.NullReferenceException e) {}
        } while (file2.Peek() != -1);

        file2.Close();
        inputfield.text = "";
    }

    public void JSONExport(string filename)
    {
        System.IO.StreamWriter jsondump = new System.IO.StreamWriter(filename + ".json");
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.Append("{\n");

        sb.Append("\t\"seed\": \""+seed+"\",\n");

        sb.Append("\t\"blobs\": [" + "\n");
        foreach (GameObject blob in BlobManager.blobs)
        {
            sb.Append("\t\t{\"id\": \"" + blob.GetComponent<BlobLogic>().getID() + "\",");
            sb.Append(" \"energy\": \"" + blob.GetComponent<BlobLogic>().getEnergy() + "\",");
            sb.Append(" \"x\": \"" + blob.transform.position.x + "\",");
            sb.Append(" \"y\": \"" + blob.transform.position.y + "\",");
            sb.Append(" \"angle\": \"" + blob.GetComponent<BlobLogic>().getAngle() + "\",");
            sb.Append(" \"dna\": \"" + blob.GetComponent<BlobDNA>().getDNA() + "\"},\n");
        }
        if (sb.Length > 0)
        {
            sb.Length -= 2;
            sb.Append("\n");
        }
        sb.Append("\t]," + "\n");

        sb.Append("\t\"foods\": [" + "\n");
        foreach (GameObject f in FoodManager.foods)
        {
            sb.Append("\t\t{\"x\": \"" + f.transform.position.x + "\",");
            sb.Append(" \"y\": \"" + f.transform.position.y + "\"},\n");
        }
        if (sb.Length > 0)
        {
            sb.Length -= 2;
            sb.Append("\n");
        }
        sb.Append("\t]" + "\n");

        sb.Append("}\n");

        jsondump.Write(sb.ToString());
        jsondump.Close();
    }
}
