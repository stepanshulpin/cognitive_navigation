using System.Collections.Generic;

namespace AI.GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        public Generation GetCurrentGeneration() {
            return population.Generation;
        }

        public GeneticAlgorithm(GeneticAlgorithmParams parameters)
        {
            this.parameters = parameters;
            random = new Utils.Random();
        }

        public void initialize()
        {
            population = new Population();
            List<IChromosome> individuals = new List<IChromosome>();
            for (int ind = 0; ind < parameters.GenerationSize; ind++)
            {
                double[] genes = new double[parameters.ChromosomeSize];
                for (int index = 0; index < parameters.ChromosomeSize; index++)
                {
                    genes[index] = random.Randomize(parameters.MinGenValue, parameters.MaxGenValue);
                }
                individuals.Add(new NumericChromosome(genes));
            }
            Generation generation = new Generation(population.Size, individuals);
            population.RegisterNewGeneration(generation);
        }

        public void newGeneration()
        {
            List<IChromosome> children = new List<IChromosome>();
            for (int i = 0; i < parameters.ChildrenSize; i++)
            {
                List<IChromosome> parents = tournamentSelection();
                children.Add(linearRecombination(parents));
            }
            List<IChromosome> previous = GetCurrentGeneration().Individuals;
            termBoundsMutation(previous, children);
            Generation newGeneration = eliteDraft(children);
            population.RegisterNewGeneration(newGeneration);
        }

        public List<IChromosome> tournamentSelection()
        {
            ISelection tournament = new TournamentSelection(parameters.SelectParentsTournamentSize, random);
            return tournament.Select(2, GetCurrentGeneration());
        }

        public IChromosome linearRecombination(List<IChromosome> parents)
        {
            IRecombination recombination = new LinearRecombination(random);
            return recombination.Recombine(parents);
        }

        public void termBoundsMutation(List<IChromosome> previous, List<IChromosome> children)
        {
            IMutation mutation = new TermBoundsMutation(random, parameters.MinGenValue, parameters.MaxGenValue);
            foreach (IChromosome chromosome in previous)
            {
                mutation.Mutate(chromosome, parameters.MutationProbability);
            }
            foreach (IChromosome chromosome in children)
            {
                mutation.Mutate(chromosome, parameters.MutationProbability);
            }
        }

        public Generation eliteDraft(List<IChromosome> children)
        {
            IGeneraionDraft draft = new EliteDraft(random, parameters.DraftPart, parameters.GenerationSize);
            return draft.Produce(GetCurrentGeneration(), children);            
        }

        private Population population;
        private GeneticAlgorithmParams parameters;
        private Utils.Random random;

    }


    public class GeneticAlgorithmParams
    {
        public int ChromosomeSize { get => chromosomeSize; set => chromosomeSize = value; }
        public int GenerationSize { get => generationSize; set => generationSize = value; }
        public double MinGenValue { get => minGenValue; set => minGenValue = value; }
        public double MaxGenValue { get => maxGenValue; set => maxGenValue = value; }
        public double MutationProbability { get => mutationProbability; set => mutationProbability = value; }
        public double DraftPart { get => draftPart; set => draftPart = value; }
        public int ChildrenSize { get => childrenSize; set => childrenSize = value; }
        public int SelectParentsTournamentSize { get => selectParentsTournamentSize; set => selectParentsTournamentSize = value; }

        public GeneticAlgorithmParams()
        {
            chromosomeSize = 0;
            generationSize = 0;
            minGenValue = 0;
            maxGenValue = 0;
            mutationProbability = 0;
            childrenSize = 0;
            selectParentsTournamentSize = 0;
            draftPart = 0;
        }

        public GeneticAlgorithmParams(int chromosomeSize, int generationSize, double minGenValue, double maxGenValue, double mutationProbability, int childrenSize, int selectParentsTournamentSize, double draftPart)
        {
            this.chromosomeSize = chromosomeSize;
            this.generationSize = generationSize;
            this.minGenValue = minGenValue;
            this.maxGenValue = maxGenValue;
            this.mutationProbability = mutationProbability;
            this.childrenSize = childrenSize;
            this.selectParentsTournamentSize = selectParentsTournamentSize;
            this.draftPart = draftPart;
        }

        private int chromosomeSize;
        private int generationSize;
        private double minGenValue;
        private double maxGenValue;
        private double mutationProbability;
        private int childrenSize;
        private int selectParentsTournamentSize;
        private double draftPart;

    }
}
