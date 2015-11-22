using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class FoodSpeed : MonoBehaviour {
	void Update () {
        FoodManager.foodSpawnRate = 100 / this.GetComponent<Slider>().value;
	}
}