using System;
using System.Collections.Generic;

namespace AI.GeneticAlgorithm
{
    class EliteDraft : IGeneraionDraft
    {
        EliteDraft(Utils.Random random, double part, int newSize)
        {
            this.part = part;
            this.random = random;
            this.newSize = newSize;
        }

        public Generation Produce(Generation generation, List<IChromosome> newbies)
        {
            ISelection selection = new EliteSelection();
            List<IChromosome> previous = new List<IChromosome>();
            previous.AddRange(generation.Individuals);
            previous.AddRange(newbies);
            List<IChromosome> newChromosomes = selection.Select((int)Math.Floor(part * generation.Individuals.Count), generation);
            newChromosomes.AddRange(previous);
            int tournamentSize = newSize - newChromosomes.Count;
            for (int i = 0; i < tournamentSize; i++)
            {
                int newInd = this.random.Next(previous.Count);
                newChromosomes.Add(previous[newInd]);
            }
            int newIndex = generation.Index + 1;
            return new Generation(newIndex, newChromosomes);
        }

        private double part;
        private Utils.Random random;
        private int newSize;
    }
}
