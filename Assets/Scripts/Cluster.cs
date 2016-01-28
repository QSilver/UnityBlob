using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    class Cluster
    {
        float x = 0, y = 0;
        float timer = 0;

        public Cluster()
        {
            this.x = ((float)Main.random.NextDouble() - 0.5f) * (FoodManager.foodSpawnSize - FoodManager.foodSpawnDiameter);
            this.y = ((float)Main.random.NextDouble() - 0.5f) * (FoodManager.foodSpawnSize - FoodManager.foodSpawnDiameter);
            this.timer = ((float)Main.random.NextDouble()) * 60 + 60;
        }

        public float getx()
        {
            return this.x;
        }

        public float gety()
        {
            return this.y;
        }

        public bool UpdateTimer()
        {
            this.timer -= 1;
            return (this.timer > 0);
        }
    }
}
