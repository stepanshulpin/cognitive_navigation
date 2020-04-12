using System;
using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms {
    public class TrapezoidalTerm : Term {
        public TrapezoidalTerm(string name, double a, double b, double c, double d) : base(name) {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        public override double Membership(double x) {
            if (x <= a || x >= d) {
                return 0.0;
            }
            if (x < b) {
                return (x - a) / (b - a);
            }
            if (x > c) {
                return (d - x) / (d - c);
            }
            return 1.0;
        }

        public override List<double> GetGenericParameters()
        {
            List<double> parameters = new List<double>();
            parameters.Add(a);
            parameters.Add(b);
            parameters.Add(c);
            parameters.Add(d);
            return parameters;
        }

        public override void Update(List<double> parameters)
        {
            if (parameters.Count != 4)
            {
                throw new ArgumentException("Invalid parameters size");
            }
            a = parameters[0];
            b = parameters[1];
            c = parameters[2];
            d = parameters[3];
        }

        public override TermType TermType()
        {
            return Terms.TermType.Trapezodial;
        }

        private double a;

        private double b;

        private double c;

        private double d;
    }
}
