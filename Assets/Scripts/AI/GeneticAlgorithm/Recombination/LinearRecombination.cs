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
            if (parent1 is NumericChromosome && parent2 is NumericChromosome)
            {
                return calc((NumericChromosome) parent1, (NumericChromosome) parent2, alpha);
            }
            else
            {
                if (parent1 is FuzzyChromosome && parent2 is FuzzyChromosome)
                {
                    return calc((FuzzyChromosome)parent1, (FuzzyChromosome)parent2, alpha);
                }
            }
            throw new Exception("Parents must be same type of chromosome");
        }

        public IChromosome calc(NumericChromosome parent1, NumericChromosome parent2, double alpha)
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

        public IChromosome calc(FuzzyChromosome parent1, FuzzyChromosome parent2, double alpha)
        {
            FuzzyChromosome child = (FuzzyChromosome) parent1.Clone();
            for (int gen = 0; gen < child.FuzzyGenes.Count; gen++)
            {
                if (parent1.FuzzyGenes[gen].TermType.Equals(parent2.FuzzyGenes[gen].TermType))
                {
                    double[] values = child.FuzzyGenes[gen].Values;
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] += alpha * (parent2.FuzzyGenes[gen].Values[i] - parent1.FuzzyGenes[gen].Values[i]);
                    }
                    child.FuzzyGenes[gen].UpdateValues(values);
                }
            }
            return child;
        }

        private double d;
        private Utils.Random random;
    }
}
