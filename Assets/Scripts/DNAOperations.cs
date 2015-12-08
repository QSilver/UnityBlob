using Assets.Scripts;
using System;
using System.Linq;

namespace Assets.Scripts
{
    static class DNAOperations
    {
        public static int DNASIZE = 3;
        public static float CROSSOVERPOINT = 0.5f;

        static Random random = Main.random;
        static double mutation_chance = 0.05;

        public static string generate(int size)
        {
            string DNA = "";

            for (int i = 0; i < size; i++)
            {
                if (random.NextDouble() < 0.5) DNA += '0';
                else DNA += '1';
            }

            return DNA;
        }

        public static string mutate(string DNA)
        {
            string newDNA = ""; char c;

            for (int i = 0; i < DNA.Length; i++)
            {
                c = DNA[i];
                if (random.NextDouble() < mutation_chance)
                    if (c == '0') c = '1';
                    else c = '0';
                newDNA += c;
            }
            return newDNA;
        }

        public static string crossover(string DNA1, string DNA2)
        {
            string newDNA = "";
            int crossoverpoint = (int)(DNASIZE * CROSSOVERPOINT);

            for (int i = 0; i < DNASIZE; i++ )
            {
                if (i < crossoverpoint)
                    newDNA += DNA1[i];
                else
                    newDNA += DNA2[i];
            }

            return newDNA;
        }
    }
}
