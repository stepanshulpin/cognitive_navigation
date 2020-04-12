using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void RegisterNewGeneration(Generation generation)
        {
            this.generation = generation;
        }

        private Generation generation;
        private int size;
    }
}
