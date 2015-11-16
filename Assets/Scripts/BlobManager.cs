using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    abstract class BlobManager
    {
        public static List<GameObject> blobs = new List<GameObject>();
        public static float blobspeed = Main.delay;
    }
}
