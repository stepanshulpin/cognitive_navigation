using System;
using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms {
    public class SigmoidalTerm : Term {
        public SigmoidalTerm(string name, double inflection, double slope) : base(name) {
            this.inflection = inflection;
            this.slope = slope;
        }

        public override double Membership(double x) {
            return 1.0 / (1.0 + Math.Exp(- slope * (x - inflection)));
        }
        public override List<double> GetGenericParameters()
        {
            List<double> parameters = new List<double>();
            parameters.Add(inflection);
            parameters.Add(slope);
            return parameters;
        }

        public override void Update(List<double> parameters)
        {
            if (parameters.Count != 2)
            {
                throw new ArgumentException("Invalid parameters size");
            }
            inflection = parameters[0];
            slope = parameters[1];
        }

        public override TermType TermType()
        {
            return Terms.TermType.Sigmoidal;
        }


        private double inflection;

        private double slope;
    }
}
