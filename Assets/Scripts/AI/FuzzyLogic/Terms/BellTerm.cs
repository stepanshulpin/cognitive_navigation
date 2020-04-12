using System;
using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms {
    public class BellTerm : Term {
        public BellTerm(string name, double center, double width, double slope) : base(name) {
            this.center = center;
            this.width = width;
            this.slope = slope;
        }

        public override double Membership(double x) {
            return 1.0 / (1 + Math.Pow(Math.Abs((x - center) / width), 2 * slope));
        }

        public override List<double> GetGenericParameters()
        {
            List<double> parameters = new List<double>();
            parameters.Add(center);
            parameters.Add(width);
            parameters.Add(slope);
            return parameters;
        }

        public override void Update(List<double> parameters)
        {
            if (parameters.Count != 3)
            {
                throw new ArgumentException("Invalid parameters size");
            }
            center = parameters[0];
            width = parameters[1];
            slope = parameters[2];
        }

        public override TermType TermType()
        {
            return Terms.TermType.Bell;
        }

        private double center;

        private double width;

        private double slope;
    }
}
