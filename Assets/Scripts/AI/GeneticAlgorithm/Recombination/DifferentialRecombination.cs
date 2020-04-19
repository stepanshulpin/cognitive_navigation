using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<IChromosome> Recombine(List<IChromosome> parents)
        {
            List<IChromosome> bestParents = parents.OrderByDescending(parent => parent.Fitness).Take(2).ToList();
            IChromosome child = Recombine(bestParents[0], bestParents[1]);
            parents.Add(child);
            return parents;
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
