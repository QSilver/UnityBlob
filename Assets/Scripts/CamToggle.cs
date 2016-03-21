using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CamToggle : MonoBehaviour
{
	void Update ()
    {
        CameraMovement.cameraToggle = this.GetComponent<Toggle>().isOn;
	}
}
