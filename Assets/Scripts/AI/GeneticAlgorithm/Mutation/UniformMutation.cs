using System;
using System.Linq;

namespace AI.GeneticAlgorithm {

    [Obsolete]
    public class UniformMutation : IMutation {
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

        public UniformMutation(Random random, double min, double max) {
            this.isBounded = true;
            this.min = min;
            this.max = max;
            this.mutableGenesIndices = null;
            this.random = random;
        }

        public void Mutate(IChromosome individual, double probability) {
            if (this.mutableGenesIndices == null || this.mutableGenesIndices.Length == 0) {
                this.mutableGenesIndices = Enumerable.Range(0, individual.Size).ToArray();
            }
            foreach (int geneIndex in this.mutableGenesIndices) {
                if (this.random.NextDouble() > probability) {
                    if (this.isBounded) {
                        //individual.RandomizeGene(geneIndex, this.min, this.max);
                    } else {
                        //individual.RandomizeGene(geneIndex);
                    }
                }
            }
        }

        private bool isBounded;

        private double min;

        private double max;

        private int[] mutableGenesIndices;

        private Random random;
    }
}
