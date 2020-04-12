using System;

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

        private double start;

        private double end;
    }
}
