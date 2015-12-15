using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;

public class BlobDisplay : MonoBehaviour
{
    public GameObject bt;
    static string display;
    static BlobLogic b;
    static int state;

    void Start()
    {
        bt.GetComponent<BehaviourTree>().Display(0);
    }

    public static void Load(BlobLogic ext)
    {
        b = ext;
        state = ext.getState();
    }

	void Update ()
    {
        bt.GetComponent<BehaviourTree>().Display(state);
        if (b != null)
        {
            display = "BlobID: " + b.getID() + System.Environment.NewLine;
            display += "Energy: " + b.getEnergy() + System.Environment.NewLine;
            display += "DNA: " + b.GetComponent<BlobDNA>().getDNA() + System.Environment.NewLine;
            display += "Range: " + b.GetComponent<BlobLogic>().getRange();
        }
        else
        {
            display = "";
            bt.GetComponent<BehaviourTree>().Display(0);
        }
        this.GetComponent<Text>().text = display;
	}
}
