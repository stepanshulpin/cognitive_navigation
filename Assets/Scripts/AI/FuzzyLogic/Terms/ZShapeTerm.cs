using System;
using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms {
    public class ZShapeTerm : Term {
        public ZShapeTerm(string name, double start, double end) : base(name) {
            this.start = start;
            this.end = end;
        }

        public override double Membership(double x) {
            if (x <= start) {
                return 1.0;
            }
            if (x <= 0.5 * (start + end)) {
                return 1.0 - 2 * Math.Pow((x - start) / (end - start), 2.0);
            }
            if (x < this.end) {
                return 2 * Math.Pow((x - end) / (end - start), 2.0);
            }
            return 0.0;
        }

        public override List<double> GetGenericParameters()
        {
            List<double> parameters = new List<double>();
            double lw = start;
            double ls = end;
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
            start = rs;
            end = 2 * rw - rs;
        }

        public override TermType TermType()
        {
            return Terms.TermType.ZShape;
        }


        private double start;

        private double end;
    }
}
