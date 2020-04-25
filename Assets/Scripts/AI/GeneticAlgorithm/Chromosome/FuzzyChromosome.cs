using AI.FuzzyLogic.Terms;

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
