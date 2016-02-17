using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    abstract class BlobManager
    {
        public static List<GameObject> blobs = new List<GameObject>();
        public static float blobspeed = Main.delay;

        public static void Place(GameObject blob, int id, float energy, float x, float y, float angle, string DNA)
        {
            GameObject clone = GameObject.Instantiate(blob, new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
            clone.AddComponent<BlobDNA>();
            clone.GetComponent<BlobDNA>().setDNA(DNA);
            clone.GetComponent<BlobLogic>().set(id, energy, x, y, angle);
            BlobManager.blobs.Add(clone);
            blobs.Add(clone);
        }
    }
}
