using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;

public class UpdateFood : MonoBehaviour {
    void Update()
    {
        this.GetComponent<Text>().text = "Food: " + FoodManager.foods.Count;
    }
}
