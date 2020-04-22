namespace AI.GeneticAlgorithm
{
    class Population
    {
        public int Size
        {
            get
            {
                return size;
            }
        }

        public Generation Generation
        {
            get
            {
                return generation;
            }
        }

        public Population()
        {
            size = 0;
        }

        public void RegisterNewGeneration(Generation generation)
        {
            this.generation = generation;
            size++;
        }

        private Generation generation;
        private int size;
    }
}
