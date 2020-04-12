namespace AI.FuzzyLogic.Terms {
    public class RectangularTerm : Term {
        public RectangularTerm(string name, double start, double end) : base(name) {
            this.start = start;
            this.end = end;
        }

        public override double Membership(double x) {
            if (start <= x && x <= end) {
                return 1.0;
            } else {
                return 0.0;
            }
        }

        private double start;

        private double end;
    }
}
