using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.UI;

public class UpdatePop : MonoBehaviour
{
	void Update ()
    {
        this.GetComponent<Text>().text = "Population: " + BlobManager.blobs.Count;
	}
}