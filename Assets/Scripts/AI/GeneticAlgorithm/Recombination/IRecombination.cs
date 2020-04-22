using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.GeneticAlgorithm
{
    public interface IRecombination
    {
        IChromosome Recombine(List<IChromosome> parents);
    }
}
