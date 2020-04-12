using System;

namespace AI.GeneticAlgorithm
{
    public interface IChromosome
    {
        int Size
        {
            get;
        }

        double Fitness
        {
            get;
            set;
        }

        double[] Genes
        {
            get;
        }

        void UpdateGenes(double[] genes);
        IChromosome Clone();
    }
}
