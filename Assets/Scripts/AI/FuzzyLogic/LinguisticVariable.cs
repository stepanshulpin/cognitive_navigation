using System.Collections.Generic;
using AI.FuzzyLogic.Terms;

namespace AI.FuzzyLogic {
    public class LinguisticVariable {
        public string Name {
            get {
                return name;
            }
        }

        public double Min {
            get {
                return min;
            }
        }

        public double Max {
            get {
                return max;
            }
        }

        public double Value {
            get {
                return value;
            }
            set {
                if (isLockedInRange) {
                    if (value >= max) {
                        this.value = max;
                    } else if (value <= min) {
                        this.value = min;
                    } else {
                        this.value = value;
                    }
                } else {
                    this.value = value;
                }
            }
        }

        public Dictionary<string, double> Fuzzy {
            get {
                return fuzzyRepresentation;
            }
        }

        public bool IsLockedInRange {
            get {
                return isLockedInRange;
            }
            set {
                isLockedInRange = value;
            }
        }

        public LinguisticVariable(string name, double minPossibleValue, double maxPossibleValue) {
            this.name = name;
            min = minPossibleValue;
            max = maxPossibleValue;
            value = 0.0;
            isLockedInRange = false;
            terms = new Dictionary<string, Term>();
            fuzzyRepresentation = new Dictionary<string, double>();
        }

        public void AddTerm(Term term) {
            terms.Add(term.Name, term);
            fuzzyRepresentation.Add(term.Name, 0.0);
        }

        public void RemoveTerm(string termName) {
            terms.Remove(termName);
            fuzzyRepresentation.Remove(termName);
        }

        public Term GetTerm(string termName) {
            return terms[termName];
        }

        public bool HasTerm(string termName) {
            return terms.ContainsKey(termName);
        }

        public Dictionary<string, double> Fuzzify() {
            return Fuzzify(value);
        }

        public Dictionary<string, double> Fuzzify(double x) {
            foreach (Term term in terms.Values) {
                fuzzyRepresentation[term.Name] = term.Membership(x);
            }
            return fuzzyRepresentation;
        }

        public double Fuzzify(double x, string termName) {
            return terms[termName].Membership(x);
        }

        private string name;

        private double min;

        private double max;

        private double value;

        private Dictionary<string, Term> terms;

        private bool isLockedInRange;

        private Dictionary<string, double> fuzzyRepresentation;
    }
}
