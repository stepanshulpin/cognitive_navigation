using System.Collections.Generic;
using System.Linq;

namespace AI.GeneticAlgorithm {
    public class EliteSelection<T> : ISelection<T> {
        public List<IGenome<T>> Select(int count, Generation<T> generation) {
            return generation.Individuals.OrderByDescending(individual => individual.Fitness).Take(count).ToList();
        }
    }
}
