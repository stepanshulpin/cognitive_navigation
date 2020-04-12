using System;

namespace AI.FuzzyLogic {
    public class Extremely : IQuantifier {
        public string Name {
            get {
                return "extremely";
            }
        }

        public QuantifierOrder Order {
            get {
                return QuantifierOrder.Extremely;
            }
        }

        public double Apply(double membership) {
            if (membership < 0.5f) {
                return 2 * Math.Pow(membership, 2.0);
            } else {
                return 1.0 - 2 * Math.Pow(1.0 - membership, 2.0f);
            }
        }
    }
}
