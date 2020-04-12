using AI.FuzzyLogic.Terms;

namespace AI.FuzzyLogic {
    public class SmallestOfMaximumDefuzzifier : IntegralDefuzzifier {
        public override double Defuzzify(Term term, double min, double max) {
            double stepSize = (max - min) / samplingRate;
            double largestMembership = 0.0;
            double smallestVariableValue = 0.0;
            for (int sample = 0; sample < samplingRate; ++sample) {
                double variableValue = min + (sample + 0.5) * stepSize;
                double termMembership = term.Membership(variableValue);
                if (termMembership > largestMembership) {
                    largestMembership = termMembership;
                    smallestVariableValue = variableValue;
                }
            }
            return smallestVariableValue;
        }
    }
}
