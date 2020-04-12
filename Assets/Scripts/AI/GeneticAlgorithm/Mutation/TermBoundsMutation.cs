using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.GeneticAlgorithm
{
    class TermBoundsMutation : IMutation
    {
        public TermBoundsMutation(Utils.Random random, double min, double max)
        {
            this.min = min;
            this.max = max;
            this.random = random;
        }

        public void Mutate(IChromosome individual, double probability)
        {

            double[] genes = individual.Genes;

            for(int geneIndex = 0; geneIndex < individual.Size; geneIndex++)
            {
                if (random.NextDouble() > probability)
                {
                    genes[geneIndex] = random.Randomize<double>(min, max);
                }
            }

            individual.UpdateGenes(genes);
        }

        private double min;

        private double max;

        private Utils.Random random;
    }
}
