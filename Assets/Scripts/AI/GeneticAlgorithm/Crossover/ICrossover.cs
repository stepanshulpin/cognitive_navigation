using System.Collections.Generic;

namespace AI.GeneticAlgorithm {
    public interface ICrossover<T> {
        int ParentsCount {
            get;
        }

        int ChildrensCount {
            get;
        }

        List<IGenome<T>> Cross(List<IGenome<T>> parents);
    }
}
