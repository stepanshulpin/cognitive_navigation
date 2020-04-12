using AI.FuzzyLogic.Terms;

namespace AI.FuzzyLogic {
    public abstract class WeightedDefuzzifier : IDefuzzifier {
        public abstract double Defuzzify(Term term, double min, double max);
    }
}

