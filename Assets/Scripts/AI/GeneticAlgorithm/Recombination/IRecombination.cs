using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.GeneticAlgorithm
{
    public interface IRecombination
    {
        List<IChromosome> Recombine(List<IChromosome> parents);
    }
}
