namespace AI.GeneticAlgorithm {
    public interface IMutation {
        void Mutate(IChromosome individual, double probability);
    }
}
