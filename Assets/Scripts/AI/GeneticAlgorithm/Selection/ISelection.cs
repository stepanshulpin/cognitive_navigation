using System.Collections.Generic;

namespace AI.GeneticAlgorithm {
    public interface ISelection {
        List<IChromosome> Select(int count, Generation generation);
    }
}
