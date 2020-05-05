using System;
using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms
{
    public class GaussianTerm : Term
    {
        public GaussianTerm(string name, double mean, double standartDeviation) : base(name)
        {
            this.mean = mean;
            this.standartDeviation = standartDeviation;
        }

        public GaussianTerm(string name) : base(name)
        {
            mean = 0;
            standartDeviation = 0;
        }

        public override double Membership(double x)
        {
            return Math.Exp(-(x - mean) * (x - mean) / (2 * standartDeviation * standartDeviation));
        }

        public override List<double> GetGenericParameters()
        {
            List<double> parameters = new List<double>();
            double lw = -Math.Abs(standartDeviation) * Math.Sqrt(-2 * Math.Log(0.5)) + mean;
            double ls = -Math.Abs(standartDeviation) * Math.Sqrt(-2 * Math.Log(0.9)) + mean;
            double rs = 2 * mean - ls;
            double rw = 2 * mean - lw;
            parameters.Add(lw);
            parameters.Add(ls);
            parameters.Add(rs);
            parameters.Add(rw);
            return parameters;
        }

        public override void Update(List<double> parameters)
        {
            double lw = parameters[0];
            double ls = parameters[1];
            double rs = parameters[2];
            double rw = parameters[3];
            mean = 0.5 * (rs + ls);
            standartDeviation = (rw - mean) / Math.Sqrt(-2 * Math.Log(0.5));
        }

        public override TermType TermType()
        {
            return Terms.TermType.Gaussian;
        }

        public override int Size()
        {
            return 2;
        }

        public override Term Clone()
        {
            return new GaussianTerm(name, mean, standartDeviation);
        }

        public override void SetValues(double[] values)
        {
            standartDeviation = values[0];
            mean = values[1];
        }

        public override double[] GetValues()
        {
            double[] values = new double[Size()];
            values[0] = standartDeviation;
            values[1] = mean;
            return values;
        }

        private double mean;

        private double standartDeviation;
    }
}
