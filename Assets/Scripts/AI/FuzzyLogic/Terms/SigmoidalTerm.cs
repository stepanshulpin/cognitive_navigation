using System;

namespace AI.FuzzyLogic.Terms {
    public class SigmoidalTerm : Term {
        public SigmoidalTerm(string name, double inflection, double slope) : base(name) {
            this.inflection = inflection;
            this.slope = slope;
        }

        public override double Membership(double x) {
            return 1.0 / (1.0 + Math.Exp(- slope * (x - inflection)));
        }

        private double inflection;

        private double slope;
    }
}
