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

        private double a;

        private double b;

        private double c;

        private double d;
    }
}
