using System;
using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms {
    public class SShapeTerm : Term {
        public SShapeTerm(string name, double start, double end) : base(name) {
            this.start = start;
            this.end = end;
        }

        public override double Membership(double x) {
            if (x <= start) {
                return 0.0;
            }
            if (x <= 0.5 * (start + end)) {
                return 2 * Math.Pow((x - start) / (end - start), 2.0);
            }
            if (x < end) {
                return 1.0 - 2 * Math.Pow((x - end) / (end - start), 2.0);
            }
            return 1.0;
        }

        public override List<double> GetGenericParameters()
        {
            List<double> parameters = new List<double>();
            double lw = start;
            double ls = 0.9 * (end - start) + start;
            double rs = end;
            double rw = 2 * end - start;
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
            start = 2 * lw - ls;
            end = ls;
        }

        public override TermType TermType()
        {
            return Terms.TermType.SShape;
        }

        private double start;

        private double end;
    }
}
