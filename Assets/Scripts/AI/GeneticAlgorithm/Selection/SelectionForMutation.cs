using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.GeneticAlgorithm
{
    public class SelectionForMutation : ISelection
    {
        public List<IChromosome> Select(int count, Generation generation)
        {
            return generation.Individuals.OrderBy(individual => individual.Fitness).Take(count).ToList();
        }
    }
}
