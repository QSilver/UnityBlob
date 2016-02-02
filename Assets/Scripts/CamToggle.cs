using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CamToggle : MonoBehaviour
{
	
	// Update is called once per frame
	void Update ()
    {
        CameraMovement.cameraToggle = this.GetComponent<Toggle>().isOn;
	}
}
