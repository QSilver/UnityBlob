using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;

public class BlobDisplay : MonoBehaviour
{
    static string display;
    static BlobLogic b;

    public static void Load(BlobLogic ext)
    {
        b = ext;
    }

	void Update ()
    {
        if (b != null)
        {
            display = "BlobID: " + b.getID() + System.Environment.NewLine;
            display += "Energy: " + b.getEnergy() + System.Environment.NewLine;
            display += "DNA: " + b.GetComponent<BlobDNA>().getDNA();
        }
        else display = "";
        this.GetComponent<Text>().text = display;
	}
}
