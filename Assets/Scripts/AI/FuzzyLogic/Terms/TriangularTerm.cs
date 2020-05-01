using System;
using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms {
    public class TriangularTerm : Term {
        public TriangularTerm(string name, double a, double b, double c) : base(name) {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public TriangularTerm(string name) : base(name)
        {
            a = 0;
            b = 0;
            c = 0;
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
            double lw = 0.5 * (b - a) + a;
            double ls = 0.9 * (b - a) + a;
            double rs = 0.1 * (c - b) + b;
            double rw = 0.5 * (c - b) + b;
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
            b = 0.5 * (rs + ls);
            a = 2 * lw - b;
            c = 2 * rw - b;
        }

        public override TermType TermType()
        {
            return Terms.TermType.Triangular;
        }

        public override int Size()
        {
            return 3;
        }

        public override Term Clone()
        {
            return new TriangularTerm(name, a, b, c);
        }

        public override void SetValues(double[] values)
        {
            a = values[0];
            b = values[1];
            c = values[2];
        }
        public override double[] GetValues()
        {
            double[] values = new double[Size()];
            values[0] = a;
            values[1] = b;
            values[2] = c;
            return values;
        }

        private double a;

        private double b;

        private double c;
    }
}
