using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    abstract class FoodManager
    {
        static float foodSpawnSize = 10f;

        static float t = 0f;
        static int maxFood = 50;
        public static float foodSpawnRate = 5f;
        public static int foodSpawnAmount = 1;
        public static float spawnspeed = Main.delay * foodSpawnRate;
        public static List<GameObject> foods = new List<GameObject>();

        public static void Spawn(GameObject food)
        {
            t += Time.deltaTime * 1000;
            if (t > spawnspeed && foods.Count < maxFood)
            {
                for (int i = 0; i <= foodSpawnAmount; i++)
                {
                    GameObject clone = GameObject.Instantiate(food, new Vector3((float)((Main.random.NextDouble() - 0.5) * foodSpawnSize), (float)((Main.random.NextDouble() - 0.5) * foodSpawnSize)), new Quaternion(0, 0, 0, 0)) as GameObject;
                    foods.Add(clone);
                    t = 0f;
                }
            }
        }
    }
}
