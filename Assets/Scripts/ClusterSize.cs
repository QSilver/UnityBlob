using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.UI;

public class ClusterSize : MonoBehaviour
{
    void Update()
    {
        FoodManager.foodSpawnDiameter = this.GetComponent<Slider>().value;
    }
}
