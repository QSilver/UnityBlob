using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnvHostility : MonoBehaviour
{
    void Update()
    {
        Main.hostility = this.GetComponent<Slider>().value;
    }
}
