using System;
using System.Collections.Generic;

namespace AI.GeneticAlgorithm
{
    public class RouletteWheelSelection : ISelection
    {
        public RouletteWheelSelection(Utils.Random random)
        {
            this.random = random;
        }
        public List<IChromosome> Select(int count, Generation generation)
        {
            double generationFitness = generation.CalculateFitness();
            List<IChromosome> selected = new List<IChromosome>();
            for (int i = 0; i < count; i++)
            {
                double expectedFitness = random.NextDouble() * generationFitness;
                IChromosome selectedIndividual = this.SelectAppropriateIndividual(generation.Individuals, expectedFitness);
                selected.Add(selectedIndividual);
            }
            return selected;
        }

        private IChromosome SelectAppropriateIndividual(List<IChromosome> individuals, double expectedFitness)
        {
            int selected = -1;
            for (int i = 0; i < individuals.Count; i++)
            {
                expectedFitness -= individuals[i].Fitness;
                if (expectedFitness < 0.0f)
                {
                    selected = i;
                    break;
                }
            }
            if (selected < 0)
            {
                selected = individuals.Count - 1;
            }
            return individuals[selected];
        }

        private Utils.Random random;
    }
}
