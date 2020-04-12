using System;
using System.Collections.Generic;

namespace AI.GeneticAlgorithm {
    public class RouletteWheelSelection<T> : ISelection<T> {
        public RouletteWheelSelection(Random random) {
            this.random = random;
        }
        public List<IGenome<T>> Select(int count, Generation<T> generation) {
            float generationFitness = generation.CalculateFitness();
            List<IGenome<T>> selected = new List<IGenome<T>>();
            for (int i = 0; i < count; i++) {
                float expectedFitness = (float)random.NextDouble() * generationFitness;
                IGenome<T> selectedIndividual = this.SelectAppropriateIndividual(generation.Individuals, expectedFitness);
                selected.Add(selectedIndividual);
            }
            return selected;
        }

        private IGenome<T> SelectAppropriateIndividual(List<IGenome<T>> individuals, float expectedFitness) {
            int selected = -1;
            for (int i = 0; i < individuals.Count; i++) {
                expectedFitness -= individuals[i].Fitness;
                if (expectedFitness < 0.0f) {
                    selected = i;
                    break;
                }
            }
            if (selected < 0) {
                selected = individuals.Count - 1;
            }
            return individuals[selected];
        }

        private Random random;
    }
}
