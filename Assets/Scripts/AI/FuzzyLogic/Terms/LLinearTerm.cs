namespace AI.FuzzyLogic.Terms {
    public class LLinearTerm : Term {
        public LLinearTerm(string name, double start, double end) : base(name) {
            this.start = start;
            this.end = end;
        }

        public override double Membership(double x) {
            if (x < start) {
                return 1.0;
            }
            if (x > end) {
                return 0.0;
            }
            return (end - x) / (end - start);
        }

        private double start;

        private double end;
    }
}
