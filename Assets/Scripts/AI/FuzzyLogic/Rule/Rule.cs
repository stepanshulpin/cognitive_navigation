using System;

namespace AI.FuzzyLogic {
    public class Rule {
        public double Weight {
            get {
                return weight;
            }
            set {
                weight = value;
            }
        }

        public Consequent Consequent {
            get {
                return consequent;
            }
        }

        public double ActivationDegree {
            get {
                return weight * antecedent.ActivationDegree;
            }
        }

        public Rule(Antecedent antecedent, Consequent consequent) {
            this.antecedent = antecedent;
            this.consequent = consequent;
            weight = 1.0f;
        }

        public double Activate(TNorm conjuction, SNorm disjunction) {
            antecedent.Activate(conjuction, disjunction);
            return ActivationDegree;
        }

        public override string ToString() {
            return string.Format("If {0} then {1}", antecedent.ToString(), consequent.ToString());
        }

        private Antecedent antecedent;

        private Consequent consequent;

        private double weight;
    }
}
