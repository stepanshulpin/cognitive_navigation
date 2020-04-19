using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.GeneticAlgorithm
{
    interface IGeneraionDraft
    {
        Generation Produce(Generation generation, List<IChromosome> newbies);
    }
}
