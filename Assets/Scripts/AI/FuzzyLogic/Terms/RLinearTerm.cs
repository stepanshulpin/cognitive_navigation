namespace AI.FuzzyLogic.Terms {
    public class RLinearTerm : Term {
        public RLinearTerm(string name, double start, double end) : base(name) {
            this.start = start;
            this.end = end;
        }

        public override double Membership(double x) {
            if (x < start) {
                return 0.0;
            }
            if (x > end) {
                return 1.0;
            }
            return (x - start) / (end - start);
        }

        private double start;

        private double end;
    }
}
