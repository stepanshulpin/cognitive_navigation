using System;

namespace AI.GeneticAlgorithm
{
    public class TermBoundsMutation : IMutation
    {

        public TermBoundsMutation()
        {
            min = 0;
            max = 1;
            random = new Utils.Random();
        }

        public TermBoundsMutation(Utils.Random random, double min, double max)
        {
            this.min = min;
            this.max = max;
            this.random = random;
        }

        public TermBoundsMutation(Utils.Random random)
        {
            min = 0;
            max = 1;
            this.random = random;
        }

        public void Mutate(IChromosome individual, double probability)
        {

            double[] genes = individual.Genes;

            for(int geneIndex = 0; geneIndex < individual.Size; geneIndex++)
            {
                if (random.NextDouble() < probability)
                {
                    if (individual.hasBounds())
                    {
                        Tuple<double, double> minMax = individual.getRanges(geneIndex);
                        genes[geneIndex] = random.Randomize<double>(minMax.Item1, minMax.Item2);
                    }
                    else
                    {
                        genes[geneIndex] = random.Randomize<double>(min, max);
                    }
                }
            }

            individual.UpdateGenes(genes);
        }

        private double min;

        private double max;

        private Utils.Random random;
    }
}
