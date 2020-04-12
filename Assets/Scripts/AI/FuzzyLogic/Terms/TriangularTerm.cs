using System;

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

        private double a;

        private double b;

        private double c;
    }
}
