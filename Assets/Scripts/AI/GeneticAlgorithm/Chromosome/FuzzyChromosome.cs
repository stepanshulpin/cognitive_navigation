using AI.FuzzyLogic.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.GeneticAlgorithm
{
    class FuzzyChromosome : NumericChromosome
    {

        TermType TermType
        {
            get
            {
                return termType;
            }
        }

        public void EvolveTo(TermType termType)
        {
            this.termType = termType;
        }

        public FuzzyChromosome(TermType termType) : base()
        {
            this.termType = termType;
        }

        public FuzzyChromosome(TermType termType, double[] genes) : base(genes)
        {
            this.termType = termType;
        }

        private TermType termType;
    }
}
