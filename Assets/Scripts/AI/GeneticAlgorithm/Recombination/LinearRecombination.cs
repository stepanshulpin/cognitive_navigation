using System;
using System.Collections.Generic;

namespace AI.GeneticAlgorithm
{
    public class LinearRecombination : IRecombination
    {
        public LinearRecombination(Utils.Random random)
        {
            this.random = random;
            d = 0.25;
        }

        public LinearRecombination(Utils.Random random, double d)
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
            double alpha = random.Randomize(-d, 1 + d);
            if (parent1.Size != parent2.Size)
            {
                throw new Exception("Parents must have same size!");
            }
            return calc(parent1, parent2, alpha);
        }

        public IChromosome calc(IChromosome parent1, IChromosome parent2, double alpha)
        {
            int size = parent1.Size;
            IChromosome child = parent1.Clone();
            double[] genes = child.Genes;
            double[] parent1Genes = parent1.Genes;
            double[] parent2Genes = parent2.Genes;
            for (int gen = 0; gen < size; gen++)
            {
                genes[gen] += alpha * (parent2Genes[gen] - parent1Genes[gen]);
            }
            child.UpdateGenes(genes);
            return child;
        }

        private double d;
        private Utils.Random random;
    }
}
