using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    abstract class FoodManager
    {
        static float t = 0f;
        static int maxFood = 50;
        public static float foodSpawnRate = 5f;
        public static float spawnspeed = Main.delay * foodSpawnRate;
        public static List<GameObject> foods = new List<GameObject>();

        public static void Spawn(GameObject food)
        {
            t += Time.deltaTime * 1000;
            if (t > spawnspeed && foods.Count < maxFood)
            {
                GameObject clone = GameObject.Instantiate(food, new Vector3(((float)Random.Range(-5000, 5000)) / 1000, ((float)Random.Range(-5000, 5000)) / 1000), new Quaternion(0, 0, 0, 0)) as GameObject;
                foods.Add(clone);
                t = 0f;
            }
        }
    }
}
