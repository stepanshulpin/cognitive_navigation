using System.Collections.Generic;

namespace AI.GeneticAlgorithm {
    public interface ISelection<T> {
        List<IGenome<T>> Select(int count, Generation<T> generation);
    }
}
