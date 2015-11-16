using UnityEngine;
using UnityEngine.UI;

public class SimSpeed : MonoBehaviour {

    void Update ()
    {
        Main.delay = 1000/this.GetComponent<Slider>().value;
    }
}
