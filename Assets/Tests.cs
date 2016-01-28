using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Assertions;

namespace Assets
{
    class Tests
    {

        public void DNATest()
        {
            String DNA1 = DNAOperations.generate(DNAOperations.DNASIZE);
            Assert.AreEqual(DNA1.Length, DNAOperations.DNASIZE);
        }
        public void SameCrossTest()
        {
            String DNA1 = DNAOperations.generate(DNAOperations.DNASIZE);
            String DNA2 = DNAOperations.crossover(DNA1, DNA1);

            Assert.AreEqual(DNA1, DNA2);
        }

        public void CrossTest()
        {
            String DNA1 = DNAOperations.generate(DNAOperations.DNASIZE);
            String DNA2 = DNAOperations.generate(DNAOperations.DNASIZE);
            String DNA3 = DNAOperations.crossover(DNA1, DNA2);

            Assert.AreNotEqual(DNA1, DNA2);
            int cross = (int)(DNAOperations.DNASIZE * DNAOperations.CROSSOVERPOINT);
            DNA1 = DNA1.Substring(0, cross);
            DNA2 = DNA1.Substring(cross);
            String DNA31 = DNA3.Substring(0, cross);
            String DNA32 = DNA3.Substring(cross);

            Assert.AreEqual(DNA1, DNA31);
            Assert.AreEqual(DNA2, DNA32);
        }

        public void MutateTest()
        {
            String DNA1 = DNAOperations.generate(DNAOperations.DNASIZE);
            String DNA2 = DNAOperations.mutate(DNA1);

            while (DNA1.CompareTo(DNA2) == 0)
            {
                DNA2 = DNAOperations.mutate(DNA1);
            }

            Assert.AreEqual(DNA1, DNA2);
        }
    }
}
