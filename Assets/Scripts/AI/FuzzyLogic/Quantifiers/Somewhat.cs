using System;

namespace AI.FuzzyLogic {
    public class Somewhat : IQuantifier {
        public string Name {
            get {
                return "somewhat";
            }
        }

        public QuantifierOrder Order {
            get {
                return QuantifierOrder.Somewhat;
            }
        }

        public double Apply(double membership) {
            return Math.Sqrt(membership);
        }
    }
}
