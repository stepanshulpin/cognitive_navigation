using AI.FuzzyLogic.Terms;
using System;

namespace AI.GeneticAlgorithm
{
    public class FuzzyGene
    {
        public TermType TermType
        {
            get
            {
                return termType;
            }
        }

        public double[] Values { get => values; }
        public int Size { get => size; }
        public double MinValue { get => minValue; }
        public double MaxValue { get => maxValue; }

        public FuzzyGene(TermType termType, double minValue, double maxValue, int size)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.size = size;
            this.termType = termType;
            values = new double[size];
        }

        public FuzzyGene(double minValue, double maxValue, int size)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.size = size;
            values = new double[size];
        }

        public FuzzyGene(double[] values, double minValue, double maxValue, int size)
        {
            this.values = values;
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.size = size;
        }

        public void UpdateValues(double[] values)
        {
            if (size != values.Length)
            {
                throw new Exception("Values size must be same with gene size");
            }
            values.CopyTo(this.values, 0);
        }

        public FuzzyGene Clone()
        {
            FuzzyGene res = new FuzzyGene(minValue, maxValue, size);
            values.CopyTo(res.values, 0);
            return res;
        }

        private double[] values;
        private double minValue;
        private double maxValue;
        private int size;
        private TermType termType;        
    }
}
