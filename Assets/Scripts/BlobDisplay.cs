using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlobDisplay : MonoBehaviour {

    static string display;
    static Blob b;

    public static void Load(Blob ext)
    {
        b = ext;
    }


	void Update () {
        if (b != null)
        {
            display = "BlobID: " + b.getID() + System.Environment.NewLine;
            display += "Energy: " + b.getEnergy();
        }
        else display = "";
        this.GetComponent<Text>().text = display;
	}
}
