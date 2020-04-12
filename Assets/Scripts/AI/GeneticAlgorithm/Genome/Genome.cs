using System;

namespace AI.GeneticAlgorithm {
    [Obsolete]
    public class Genome<T> : IGenome<T> {
        public int Length {
            get {
                return this.genes.Length;
            }
        }

        public float Fitness {
            get {
                return this.fitness;
            }
            set {
                this.fitness = value;
            }
        }

        public T[] Genes {
            get {
                return this.genes;
            }
        }

        public Genome(int length) : this(length, new Utils.Random()) {
        }

        public Genome(T[] genes) : this(genes, new Utils.Random()) {
        }

        public Genome(T[] genes, Utils.Random random) {
            this.genes = genes;
            this.random = random;
            this.fitness = 0.0f;
        }

        public Genome(int length, Utils.Random random) {
            this.genes = new T[length];
            this.random = random;
            this.fitness = 0.0f;
        }

        public void RandomizeGene(int geneIndex) {
            this.genes[geneIndex] = this.random.Randomize<T>();
        }

        public void RandomizeGene(int geneIndex, T min, T max) {
            this.genes[geneIndex] = this.random.Randomize<T>(min, max);
        }

        public void RandomizeGenes(int randomizeFrom, int randomizeTo) {
            for (int index = randomizeFrom; index < randomizeTo; index++) {
                this.genes[index] = this.random.Randomize<T>();
            }
        }

        public void RandomizeGenes(int randomizeFrom, int randomizeTo, T min, T max) {
            for (int index = randomizeFrom; index < randomizeTo; index++) {
                this.genes[index] = this.random.Randomize(min, max);
            }
        }

        public void ReplaceGene(int index, T gene) {
            this.genes[index] = gene;
        }

        public void ReplaceGenes(int replaceFrom, T[] genes) {
            if (replaceFrom + genes.Length - 1 < this.genes.Length) {
                Array.Copy(genes, 0, this.genes, replaceFrom, genes.Length - 1);
            } else {
                throw new ArgumentOutOfRangeException("End gene index is greater then genome length");
            }
        }

        public void ReplaceGenes(int replaceFrom, int replaceTo, T[] genes) {
            if (replaceTo < this.genes.Length) {
                Array.Copy(genes, replaceFrom, this.genes, replaceFrom, replaceTo - replaceFrom + 1);
            } else {
                throw new ArgumentOutOfRangeException("End gene index is greater then genome length");
            }
        }

        public IGenome<T> Clone() {
            Genome<T> clone = new Genome<T>(this.Length);
            Array.Copy(this.genes, clone.genes, this.Length);
            return clone;
        }

        private T[] genes;

        private float fitness;

        private Utils.Random random;
    }
}
