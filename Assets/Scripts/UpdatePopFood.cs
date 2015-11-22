using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;

public class UpdatePopFood : MonoBehaviour {
    void Update()
    {
        this.GetComponent<Text>().text = "Food/Pop: " + ((float)FoodManager.foods.Count/BlobManager.blobs.Count).ToString("0.00");
    }
}
