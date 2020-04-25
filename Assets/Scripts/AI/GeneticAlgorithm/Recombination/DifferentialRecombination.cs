using System;
using System.Collections.Generic;

namespace AI.GeneticAlgorithm
{
    public class DifferentialRecombination : IRecombination
    {
        public DifferentialRecombination(Utils.Random random)
        {
            this.random = random;
            d = 0.25;
        }

        public DifferentialRecombination(Utils.Random random, double d)
        {
            this.random = random;
            this.d = d;
        }

        public IChromosome Recombine(List<IChromosome> parents)
        {
            if (parents.Count != 2)
            {
                throw new Exception("Recombination can be appliedd only for two parents!");
            }
            return Recombine(parents[0], parents[1]);
        }

        private IChromosome Recombine(IChromosome parent1, IChromosome parent2)
        {            
            if (parent1.Size != parent2.Size)
            {
                throw new Exception("Parents must have same size!");
            }
            int size = parent1.Size;
            IChromosome child = parent1.Clone();
            for (int gen = 0; gen < size; gen++)
            {
                double alpha = random.Randomize(-d, 1 + d);
                child.Genes[gen] += alpha * (parent2.Genes[gen] - parent1.Genes[gen]);
            }
            return child;
        }

        private double d;
        private Utils.Random random;
    }
}
