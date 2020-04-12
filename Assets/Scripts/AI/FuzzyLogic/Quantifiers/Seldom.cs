using System;

namespace AI.FuzzyLogic {
    public class Seldom : IQuantifier {
        public string Name {
            get {
                return "seldom";
            }
        }

        public QuantifierOrder Order {
            get {
                return QuantifierOrder.Seldom;
            }
        }

        public double Apply(double membership) {
            if (membership < 0.5f) {
                return Math.Sqrt(0.5 * membership);
            } else {
                return 1.0 - Math.Sqrt(0.5 * (1.0 - membership));
            }
        }
    }
}
