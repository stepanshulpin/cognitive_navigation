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

        public TrapezoidalTerm(string name) : base(name)
        {
            a = 0;
            b = 0;
            c = 0;
            d = 0;
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
            double lw = 0.5 * (b - a) + a;
            double ls = 0.9 * (b - a) + a;
            double rs = 0.1 * (d - c) + c;
            double rw = 0.5 * (d - c) + c;
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
            a = 2 * lw - ls;
            b = ls;
            c = rs;
            d = 2 * rw - rs;
        }

        public override TermType TermType()
        {
            return Terms.TermType.Trapezodial;
        }

        public override int Size()
        {
            return 4;
        }

        public override Term Clone()
        {
            return new TrapezoidalTerm(name, a, b, c, d);
        }

        public override void SetValues(double[] values)
        {
            a = values[0];
            b = values[1];
            c = values[2];
            d = values[3];
        }
        public override double[] GetValues()
        {
            double[] values = new double[Size()];
            values[0] = a;
            values[1] = b;
            values[2] = c;
            values[3] = d;
            return values;
        }

        private double a;

        private double b;

        private double c;

        private double d;
    }
}
