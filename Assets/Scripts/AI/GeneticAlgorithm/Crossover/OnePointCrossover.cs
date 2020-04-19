using System;
using System.Collections.Generic;

namespace AI.GeneticAlgorithm {
    [Obsolete]
    public class OnePointCrossover<T> : ICrossover<T> {
        public int ParentsCount {
            get {
                return 2;
            }
        }

        public int ChildrensCount {
            get {
                return 2;
            }
        }

        public OnePointCrossover(int swapPointIndex) {
            this.swapPointIndex = swapPointIndex;
        }

        public List<IGenome<T>> Cross(List<IGenome<T>> parents) {
            IGenome<T> firstParent = parents[0];
            IGenome<T> secondParent = parents[1];
            return new List<IGenome<T>>() {
                this.CreateChild(firstParent, secondParent),
                this.CreateChild(secondParent, firstParent)
            };
        }

        private IGenome<T> CreateChild(IGenome<T> leftParent, IGenome<T> rightParent) {
            IGenome<T> child = leftParent.Clone();
            child.ReplaceGenes(swapPointIndex + 1, rightParent.Genes.Length - 1, rightParent.Genes);
            return child;
        }

        private int swapPointIndex;
    }
}
