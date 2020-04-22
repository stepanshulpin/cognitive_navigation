using System;
using System.Collections.Generic;
using System.Linq;

namespace AI.GeneticAlgorithm
{
    public class Generation
    {
        public int Index
        {
            get
            {
                return index;
            }
        }

        public long Timestamp
        {
            get
            {
                return timestamp;
            }
        }

        public List<IChromosome> Individuals
        {
            get
            {
                return individuals;
            }
        }

        public Generation(int generationIndex, List<IChromosome> individuals)
        {
            this.index = generationIndex;
            this.timestamp = DateTime.Now.Ticks;
            this.individuals = individuals;
        }

        public double CalculateFitness()
        {
            return individuals.Sum(individual => individual.Fitness);
        }

        private int index;
        private List<IChromosome> individuals;
        private long timestamp;
    }
}
