using AI.FuzzyLogic.Terms;
using System;

namespace AI.GeneticAlgorithm
{
    public class FuzzyGene
    {
        public double[] Values { get => term.GetValues(); }
        public int Size { get => term.Size(); }
        public double MinValue { get => minValue; }
        public double MaxValue { get => maxValue; }
        public Term Term { get => term; set => term = value; }
        public TermType TermType { get => term.TermType(); }

        public FuzzyGene(TermType termType, string name, double minValue, double maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            term = TermHelper.instantiate(termType, name);
        }

        public void UpdateValues(double[] values)
        {
            if (term.Size() != values.Length)
            {
                throw new Exception("Values size must be same with gene size");
            }
            term.SetValues(values);
        }

        public FuzzyGene Clone()
        {
            FuzzyGene res = new FuzzyGene(term.TermType(), term.Name, minValue, maxValue);
            res.Term = term.Clone();
            return res;
        }

        private double minValue;
        private double maxValue;
        private Term term;
    }
}
