using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI.FuzzyLogic {
    public class Statement {
        public LinguisticVariable Variable {
            get {
                return variable;
            }
        }

        public string Term {
            get {
                return term;
            }
            set {
                term = value;
            }
        }

        public List<IQuantifier> Quantifiers {
            get {
                return quantifiers;
            }
        }

        public double Probability {
            get {
                return probability;
            }
        }

        public Statement(LinguisticVariable variable) {
            this.variable = variable;
            term = null;
            quantifiers = new List<IQuantifier>();
        }

        public void AddQuantifier(IQuantifier quantifier) {
            if (quantifiers.Count > 0) {
                if (quantifiers.Last().Order <= quantifier.Order) {
                    throw new ArgumentException("Can't add quantifier to statement. Quantifiers order mismatch");
                }
            }
            quantifiers.Add(quantifier);
        }

        public double CalculateProbability() {
            probability = variable.Fuzzy[term];
            int quantifiersCount = quantifiers.Count - 1;
            for (int i = quantifiersCount; i >= 0; i--) {
                probability = quantifiers[i].Apply(probability);
            }
            return probability;
        }

        public override string ToString() {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0} is ", variable.Name);
            foreach (IQuantifier quantifier in quantifiers) {
                stringBuilder.AppendFormat("{0} ", quantifier.Name);
            }
            stringBuilder.Append(term);
            return stringBuilder.ToString();

        }

        private LinguisticVariable variable;

        private string term;

        private List<IQuantifier> quantifiers;

        private double probability;
    }
}
