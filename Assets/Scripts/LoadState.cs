using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadState : MonoBehaviour
{
    public InputField inputname;

    public void LoadPressed()
    {
        Main.LoadState(inputname.text);
    }
}
