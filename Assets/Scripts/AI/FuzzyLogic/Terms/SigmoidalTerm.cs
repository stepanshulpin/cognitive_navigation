using System;
using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms
{
    public class SigmoidalTerm : Term
    {
        public SigmoidalTerm(string name, double inflection, double slope) : base(name)
        {
            this.inflection = inflection;
            this.slope = slope;
        }

        public override double Membership(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-slope * (x - inflection)));
        }

        public override List<double> GetGenericParameters()
        {
            List<double> parameters = new List<double>();
            double lw = inflection;
            double ls = 1 / slope * Math.Log(0.1) + inflection;
            double rs = 2 * ls - lw;
            double rw = 2 * rs - lw;
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
            inflection = lw;
            slope = -Math.Log(0.1) / (ls - inflection);
        }

        public override TermType TermType()
        {
            return Terms.TermType.Sigmoidal;
        }

        private double slope;
        private double inflection;
    }
}
