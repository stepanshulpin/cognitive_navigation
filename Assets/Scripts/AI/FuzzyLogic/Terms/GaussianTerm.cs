using System;
using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms {
    public class GaussianTerm : Term {
        public GaussianTerm(string name, double mean, double standartDeviation) : base(name) {
            this.mean = mean;
            this.standartDeviation = standartDeviation;
        }

        public override double Membership(double x) {
            return Math.Exp(-(x - mean) * (x - mean) / (2 * standartDeviation * standartDeviation));
        }

        public override List<double> GetGenericParameters()
        {
            List<double> parameters = new List<double>();
            parameters.Add(mean);
            parameters.Add(standartDeviation);
            return parameters;
        }

        public override void Update(List<double> parameters)
        {
            if (parameters.Count != 2)
            {
                throw new ArgumentException("Invalid parameters size");
            }
            mean = parameters[0];
            standartDeviation = parameters[1];
        }

        public override TermType TermType()
        {
            return Terms.TermType.Gaussian;
        }

        private double mean;

        private double standartDeviation;
    }
}
