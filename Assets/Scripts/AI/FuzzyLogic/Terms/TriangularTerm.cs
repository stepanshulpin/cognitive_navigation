using System;
using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms {
    public class TriangularTerm : Term {
        public TriangularTerm(string name, double a, double b, double c) : base(name) {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public override double Membership(double x) {
            if (x <= a || x >= c) {
                return 0.0;
            }
            if (Math.Abs(x - b) < double.Epsilon) {
                return 1.0;
            }
            if (x < b) {
                return (x - a) / (b - a);
            } else {
                return (c - x) / (c - b);
            }
        }
        public override List<double> GetGenericParameters()
        {
            List<double> parameters = new List<double>();
            parameters.Add(a);
            parameters.Add(b);
            parameters.Add(c);
            return parameters;
        }

        public override void Update(List<double> parameters)
        {
            if (parameters.Count != 3)
            {
                throw new ArgumentException("Invalid parameters size");
            }
            a = parameters[0];
            b = parameters[1];
            c = parameters[2];
        }

        public override TermType TermType()
        {
            return Terms.TermType.Triangular;
        }


        private double a;

        private double b;

        private double c;
    }
}
