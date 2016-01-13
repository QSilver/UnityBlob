using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;

public class BlobDisplay : MonoBehaviour
{
    static string display;
    static BlobLogic b;
    static int state;

    void Start()
    {
    }

    public static void Load(BlobLogic ext)
    {
        b = ext;
        state = ext.getState();
    }

	void Update ()
    {
        if (b != null)
        {
            display =  "BlobID: " + b.getID() + System.Environment.NewLine;
            display += "Energy: " + b.getEnergy() + System.Environment.NewLine;
            display += "DNA: " + b.GetComponent<BlobDNA>().getDNA() + System.Environment.NewLine;
            display += "Range: " + b.GetComponent<BlobLogic>().getRange() + System.Environment.NewLine;
            display += "To Reprod: " + b.GetComponent<BlobLogic>().getReprod();
        }
        else
        {
            display = "";
        }
        this.GetComponent<Text>().text = display;
	}
}
