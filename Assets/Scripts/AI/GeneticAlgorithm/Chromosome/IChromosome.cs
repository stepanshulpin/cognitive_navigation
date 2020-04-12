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

        IChromosome Clone();
    }
}
