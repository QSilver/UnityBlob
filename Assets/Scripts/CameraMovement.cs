using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class CameraMovement : MonoBehaviour 
{
    private float speed = 5f;
    public GameObject blob;

    Plane XYPlane = new Plane(Vector3.forward, Vector3.zero);
    private LineRenderer m_LineRenderer;

    private Vector3 GetMouse()
    {
        float distance;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (XYPlane.Raycast(ray, out distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);

            hitPoint.z = 0;
            return hitPoint;
        }

        return Vector3.zero;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Main.gameState == 1 && Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 mp = GetMouse();

            if (Mathf.Abs(mp.x) < FoodManager.foodSpawnSize / 2 && Mathf.Abs(mp.y) < FoodManager.foodSpawnSize / 2)
            {
                GameObject clone = GameObject.Instantiate(blob, mp, new Quaternion(0, 0, 0, 0)) as GameObject;
                clone.AddComponent<BlobDNA>();
                clone.GetComponent<BlobDNA>().setDNA(DNAOperations.generate(DNAOperations.DNASIZE));
                BlobManager.blobs.Add(clone);
            }
        }

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
        if ((Input.GetKey(KeyCode.KeypadPlus) || (Input.GetKey(KeyCode.Z))) && transform.position.z < -1)
        {
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime * 5));
        }
        if (Input.GetKey(KeyCode.KeypadMinus) || (Input.GetKey(KeyCode.X)))
        {
            transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime * 5));
        }
    }
}
