using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.UI;

public class SimSpeed : MonoBehaviour {

    void Update ()
    {
        BlobManager.delay = 1000/this.GetComponent<Slider>().value;
    }
}
