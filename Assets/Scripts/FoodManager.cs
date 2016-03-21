using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    abstract class FoodManager
    {
        public static float foodSpawnSize = 50f;

        static float t = 0f;
        public static int maxFood = 500;
        public static float foodSpawnRate = 5f;
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
                // get random cluster
                int cluster = Main.random.Next(0, foodclusternum);
                // aquire X and Y of cluster
                // add random to make it off centre
                float xpos = clusters[cluster].getx() + (((float)Main.random.NextDouble() - 0.5f) * foodSpawnDiameter);
                float ypos = clusters[cluster].gety() + (((float)Main.random.NextDouble() - 0.5f) * foodSpawnDiameter);
                // create food pellet
                GameObject clone = GameObject.Instantiate(food, new Vector3(xpos, ypos), new Quaternion(0, 0, 0, 0)) as GameObject;
                foods.Add(clone);
                t = 0f;
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

        public static void Place(GameObject food, float x, float y)
        {
            GameObject clone = GameObject.Instantiate(food, new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
            foods.Add(clone);
        }
    }
}
