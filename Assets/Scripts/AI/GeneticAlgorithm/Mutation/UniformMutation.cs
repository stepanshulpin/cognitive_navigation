using System;
using System.Linq;

namespace AI.GeneticAlgorithm {
    public class UniformMutation<T> : IMutation<T> {
        public UniformMutation(Random random) {
            this.isBounded = false;
            this.mutableGenesIndices = null;
            this.random = random;
        }

        public UniformMutation(int[] mutableGenesIndices, Random random) {
            this.isBounded = false;
            this.mutableGenesIndices = mutableGenesIndices;
            this.random = random;
        }

        public UniformMutation(Random random, T min, T max) {
            this.isBounded = true;
            this.min = min;
            this.max = max;
            this.mutableGenesIndices = null;
            this.random = random;
        }

        public void Mutate(IGenome<T> individual, float probability) {
            if (this.mutableGenesIndices == null || this.mutableGenesIndices.Length == 0) {
                this.mutableGenesIndices = Enumerable.Range(0, individual.Length).ToArray();
            }
            foreach (int geneIndex in this.mutableGenesIndices) {
                if (this.random.NextDouble() > probability) {
                    if (this.isBounded) {
                        individual.RandomizeGene(geneIndex, this.min, this.max);
                    } else {
                        individual.RandomizeGene(geneIndex);
                    }
                }
            }
        }

        private bool isBounded;

        private T min;

        private T max;

        private int[] mutableGenesIndices;

        private Random random;
    }
}
