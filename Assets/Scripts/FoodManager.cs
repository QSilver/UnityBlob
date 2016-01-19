using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    abstract class FoodManager
    {
        public static float foodSpawnSize = 50f;

        static float t = 0f;
        static int maxFood = 500;
        public static float foodSpawnRate = 5f;
        public static int foodSpawnAmount = 1;
        public static float foodSpawnDiameter = 1.5f;
        public static float foodEnergy = 30f;
        public static float spawnspeed = Main.delay * foodSpawnRate;
        public static List<GameObject> foods = new List<GameObject>();

        static int foodclusternum = 5;
        static List<Cluster> clusters = new List<Cluster>();

        public static void Cluster()
        {
            clusters.Clear();

            for (int i = 0; i < foodclusternum; i++)
            {
                clusters.Add(new Cluster());
            }
        }

        public static void Spawn(GameObject food)
        {
            t += Time.deltaTime * 1000;
            if (t > spawnspeed && foods.Count < maxFood)
            {
                for (int i = 0; i < foodSpawnAmount; i++)
                {
                    int cluster = Main.random.Next(0,foodclusternum);
                    GameObject clone = GameObject.Instantiate(food, new Vector3(clusters[cluster].getx() + (((float)Main.random.NextDouble() - 0.5f) * foodSpawnDiameter), clusters[cluster].gety() + (((float)Main.random.NextDouble() - 0.5f)) * foodSpawnDiameter), new Quaternion(0, 0, 0, 0)) as GameObject;
                    foods.Add(clone);
                    t = 0f;
                }
            }

            for (int i = 0; i < foodclusternum; i++)
            {
                if (clusters[i].UpdateTimer() == false)
                {
                    clusters.Remove(clusters[i]);
                    clusters.Add(new Cluster());
                }
            }
        }
    }
}
