namespace AI.GeneticAlgorithm {
    public interface IMutation<T> {
        void Mutate(IGenome<T> individual, float probability);
    }
}
