
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<IChromosome> tournamentSelection(int tournamentSize)
        {
            ISelection tournament = new TournamentSelection(tournamentSize, random);
            return tournament.Select(2, GetCurrentGeneration());
        }

        public IChromosome linearRecombination(List<IChromosome> parents)
        {
            IRecombination recombination = new LinearRecombination(random);
            return recombination.Recombine(parents);
        }

        public void termBoundsMutation(IChromosome individual, double probability)
        {
            IMutation mutation = new TermBoundsMutation(random, parameters.MinGenValue, parameters.MaxGenValue);
            mutation.Mutate(individual, probability);
        }

        public Generation eliteDraft(Generation generation, List<IChromosome> children, double part)
        {
            IGeneraionDraft draft = new EliteDraft(random, part, parameters.GenerationSize);
            return draft.Produce(generation, children);            
        }

        public void registerNewGeneration(Generation generation)
        {
            population.RegisterNewGeneration(generation);
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

        public GeneticAlgorithmParams()
        {
            chromosomeSize = 0;
            generationSize = 0;
            minGenValue = 0;
            maxGenValue = 0;
        }

        public GeneticAlgorithmParams(int chromosomeSize, int generationSize, double minGenValue, double maxGenValue)
        {
            this.chromosomeSize = chromosomeSize;
            this.generationSize = generationSize;
            this.minGenValue = minGenValue;
            this.maxGenValue = maxGenValue;
        }

        private int chromosomeSize;
        private int generationSize;
        private double minGenValue;
        private double maxGenValue;

    }
}
