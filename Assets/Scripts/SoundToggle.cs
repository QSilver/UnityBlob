using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
	void Update ()
    {
        Main.hasSound = this.GetComponent<Toggle>().isOn;
	}
}
