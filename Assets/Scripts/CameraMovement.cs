using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class CameraMovement : MonoBehaviour 
{
    private float speed = 5f;
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.KeypadPlus) && transform.position.z < -1)
        {
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime * 5));
        }
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime * 5));
        }
    }
}
