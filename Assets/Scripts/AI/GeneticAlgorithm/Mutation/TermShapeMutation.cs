using AI.FuzzyLogic.Terms;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AI.GeneticAlgorithm
{
    public class TermShapeMutation : IMutation
    {

        public TermShapeMutation(Utils.Random random)
        {
            this.random = random;
        }

        public TermShapeMutation()
        {
            random = new Utils.Random();
        }

        public void Mutate(IChromosome individual, double probability)
        {
            //probability * iteration
            List<FuzzyGene> fuzzyGenes = ((FuzzyChromosome)individual).FuzzyGenes;
            for(int gene = 0; gene < fuzzyGenes.Count; gene++)
            {
                if (random.NextDouble() < probability)
                {
                    mutate(fuzzyGenes[gene]);
                }
            }

        }

        public void mutate(FuzzyGene fuzzyGene)
        {
            int termType = random.Randomize(0, TermHelper.TERM_TYPES_COUNT);
            TermType type = TermHelper.getTermType(termType);
            if (!type.Equals(fuzzyGene.TermType))
            {
                Term prevTerm = fuzzyGene.Term;
                Term term = TermHelper.instantiate(type, prevTerm.Name);
                term.Update(prevTerm.GetGenericParameters());
                if (TermHelper.isKeepTolerance(prevTerm, term, fuzzyGene.MinValue, fuzzyGene.MaxValue))
                {
                    fuzzyGene.Term = term;
                }
            }
        }

        private Utils.Random random;
    }
}
