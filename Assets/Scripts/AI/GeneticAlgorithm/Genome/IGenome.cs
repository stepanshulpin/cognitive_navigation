namespace AI.GeneticAlgorithm {
    public interface IGenome<T> {
        int Length {
            get;
        }

        float Fitness {
            get;
            set;
        }

        T[] Genes {
            get;
        }

        void RandomizeGene(int geneIndex);

        void RandomizeGene(int geneIndex, T min, T max);

        void RandomizeGenes(int randomizeFrom, int randomizeTo);

        void RandomizeGenes(int randomizeFrom, int randomizeTo, T min, T max);

        void ReplaceGene(int index, T gene);

        void ReplaceGenes(int replaceFrom, T[] genes);

        void ReplaceGenes(int replaceFrom, int replaceTo, T[] genes);

        IGenome<T> Clone();
    }
}
