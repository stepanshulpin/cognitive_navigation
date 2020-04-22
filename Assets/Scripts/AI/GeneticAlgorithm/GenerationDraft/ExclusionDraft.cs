using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.GeneticAlgorithm
{
    class ExclusionDraft : IGeneraionDraft
    {

        public ExclusionDraft(Utils.Random random, double part, int newSize)
        {
            this.part = part;
            this.random = random;
            this.newSize = newSize;
        }

        public Generation Produce(Generation generation, List<IChromosome> newbies)
        {
            ISelection selection = new EliteSelection();
            List<IChromosome> previous = selection.Select((int)Math.Floor(part * generation.Individuals.Count), generation);
            previous.AddRange(newbies);
            List<IChromosome> newChromosomes = new List<IChromosome>();
            for (int i = 0; i < newSize; i++)
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
