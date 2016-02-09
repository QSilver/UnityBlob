using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SaveScript : MonoBehaviour
{
    public InputField inputname;

	public void SavePressed()
    {
        Main.SaveState(inputname.text);
    }
}