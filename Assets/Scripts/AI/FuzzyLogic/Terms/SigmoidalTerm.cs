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

        public SigmoidalTerm(string name) : base(name)
        {
            slope = 0;
            inflection = 0;
        }

        public override double Membership(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-slope * (x - inflection)));
        }

        public override List<double> GetGenericParameters()
        {
            List<double> parameters = new List<double>();
            double lw, ls, rs, rw;
            if (slope > 0)
            {
                lw = inflection;
                ls = -1 / slope * Math.Log(0.1/0.9) + inflection;
                rs = 2 * ls - lw;
                rw = 2 * rs - lw;
            } else
            {
                rw = inflection;
                rs = -1 / slope * Math.Log(0.1 / 0.9) + inflection;
                ls = rs;
                lw = 2 * ls - rw;
            }            
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
            slope = -Math.Log(0.1/0.9) / (ls - inflection);
        }

        public override TermType TermType()
        {
            return Terms.TermType.Sigmoidal;
        }

        public override int Size()
        {
            return 2;
        }

        public override Term Clone()
        {
            return new SigmoidalTerm(name, inflection, slope);
        }

        public override void SetValues(double[] values)
        {
            slope = values[0];
            inflection = values[1];
        }

        public override double[] GetValues()
        {
            double[] values = new double[Size()];
            values[0] = slope;
            values[1] = inflection;
            return values;
        }

        private double slope;
        private double inflection;
    }
}
