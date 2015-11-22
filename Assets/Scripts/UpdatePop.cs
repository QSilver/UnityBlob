using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts;

public class UpdatePop : MonoBehaviour {
	void Update () {
        this.GetComponent<Text>().text = "Population: " + BlobManager.blobs.Count;
	}
}