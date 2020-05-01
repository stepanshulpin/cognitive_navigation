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

        public override double Membership(double x)
        {
            return Math.Exp(-(x - mean) * (x - mean) / (2 * standartDeviation * standartDeviation));
        }

        public override List<double> GetGenericParameters()
        {
            List<double> parameters = new List<double>();
            double lw = -standartDeviation * Math.Sqrt(-2 * Math.Log(0.5)) + mean;
            double ls = -standartDeviation * Math.Sqrt(-2 * Math.Log(0.9)) + mean;
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

        private double mean;

        private double standartDeviation;
    }
}
