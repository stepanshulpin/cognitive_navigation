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
            parameters.Add(start);
            parameters.Add(end);
            return parameters;
        }

        public override void Update(List<double> parameters)
        {
            if (parameters.Count != 2)
            {
                throw new ArgumentException("Invalid parameters size");
            }
            start = parameters[0];
            end = parameters[1];
        }

        public override TermType TermType()
        {
            return Terms.TermType.ZShape;
        }


        private double start;

        private double end;
    }
}
