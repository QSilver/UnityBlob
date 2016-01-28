using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;

public class BlobDisplay : MonoBehaviour
{
    public GameObject BlobPanel;

    static string display;
    static BlobLogic b;
    //static int state;

    void Start()
    {
        BlobPanel.GetComponent<CanvasGroup>().alpha = 0;
    }

    public static void Load(BlobLogic ext)
    {
        b = ext;
        //state = ext.getState();
    }

	void Update ()
    {
        if (b != null)
        {
            BlobPanel.GetComponent<CanvasGroup>().alpha = 1;

            display =  "BlobID: " + b.getID() + System.Environment.NewLine;
            display += "Energy: " + b.getEnergy() + System.Environment.NewLine;
            display += "DNA: " + b.GetComponent<BlobDNA>().getDNA() + System.Environment.NewLine;
            display += "Patience: " + b.GetComponent<BlobLogic>().getPatience() + System.Environment.NewLine;
            display += "To Reprod: " + b.GetComponent<BlobLogic>().getReprod();
        }
        else
        {
            display = "";
            BlobPanel.GetComponent<CanvasGroup>().alpha = 0;
        }
        this.GetComponent<Text>().text = display;
	}
}
