using System;

namespace AI.FuzzyLogic.Terms {
    public class GaussianTerm : Term {
        public GaussianTerm(string name, double mean, double standartDeviation) : base(name) {
            this.mean = mean;
            this.standartDeviation = standartDeviation;
        }

        public override double Membership(double x) {
            return Math.Exp(-(x - mean) * (x - mean) / (2 * standartDeviation * standartDeviation));
        }

        private double mean;

        private double standartDeviation;
    }
}
