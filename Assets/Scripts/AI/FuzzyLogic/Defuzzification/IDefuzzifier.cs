using AI.FuzzyLogic.Terms;

namespace AI.FuzzyLogic {
    public interface IDefuzzifier {
        double Defuzzify(Term term, double min, double max);
    }
}
