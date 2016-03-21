using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour 
{
    private float speed = 5f;
    public GameObject blob;
    public GameObject food;
    GameObject follow;

    Plane XYPlane = new Plane(Vector3.forward, Vector3.zero);
    private LineRenderer m_LineRenderer;

    public static bool cameraToggle;

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

    void Update()
    {
        if (cameraToggle == true)
            if (follow != null)
                transform.position = new Vector3(follow.transform.position.x, follow.transform.position.y, this.transform.position.z);

        #region SelectBlob
        if (Input.GetKey(KeyCode.Mouse0))
        {
            float mindist = 10f;
            GameObject closest = null;
            Vector3 mp = GetMouse();
            foreach (GameObject b in BlobManager.blobs)
            {
                float dist = mindist + 1;
                if (b != null)
                    dist = (b.transform.position.x - mp.x) * (b.transform.position.x - mp.x) + (b.transform.position.y - mp.y) * (b.transform.position.y - mp.y);
                if (dist < mindist)
                {
                    mindist = dist;
                    closest = b;
                }
            }
            if (mindist < 0.5f)
            {
                follow = closest;
                GameObject current = BlobManager.blobs[BlobManager.blobs.IndexOf(closest)];
                current.GetComponentInChildren<SpriteRenderer>().color = new Color(0, current.GetComponent<BlobLogic>().getReprod() / 300, 0, 1);
                BlobDisplay.Load(closest.GetComponent<BlobLogic>());
            }
        }
        #endregion

        if (Main.gameState == Main.GameState.Started && Input.GetKey(KeyCode.Mouse0) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            Vector3 mp = GetMouse();

            if (Mathf.Abs(mp.x) < FoodManager.foodSpawnSize / 2 && Mathf.Abs(mp.y) < FoodManager.foodSpawnSize / 2)
            {
                if (BlobManager.blobs.Count < 1000)
                {
                    GameObject clone = GameObject.Instantiate(blob, mp, new Quaternion(0, 0, 0, 0)) as GameObject;
                    clone.AddComponent<BlobDNA>();
                    clone.GetComponent<BlobDNA>().setDNA(DNAOperations.generate(DNAOperations.DNASIZE));
                    BlobManager.blobs.Add(clone);
                }
            }
        }

        if (Input.GetKey(KeyCode.Mouse0) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            if (FoodManager.foods.Count < FoodManager.maxFood)
            {
                Vector3 mp = GetMouse();

                GameObject clone = GameObject.Instantiate(food, mp, new Quaternion(0, 0, 0, 0)) as GameObject;
                FoodManager.foods.Add(clone);
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0)); follow = null;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0)); follow = null;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0)); follow = null;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0)); follow = null;
        }
        if ((Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Equals) || (Input.GetAxis("Mouse ScrollWheel") > 0)) && transform.position.z < -1)
        {
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime * 5));
        }
        if (Input.GetKey(KeyCode.KeypadMinus) || Input.GetKey(KeyCode.Minus) || (Input.GetAxis("Mouse ScrollWheel") < 0))
        {
            transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime * 5));
        }
    }
}
