using System.Collections.Generic;
using System.Linq;

namespace AI.GeneticAlgorithm {
    public class EliteSelection : ISelection {
        public List<IChromosome> Select(int count, Generation generation) {
            return generation.Individuals.OrderByDescending(individual => individual.Fitness).Take(count).ToList();
        }
    }
}
