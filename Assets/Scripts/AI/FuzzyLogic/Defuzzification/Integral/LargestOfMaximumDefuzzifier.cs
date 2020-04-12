using AI.FuzzyLogic.Terms;

namespace AI.FuzzyLogic {
    public class LargestOfMaximumDefuzzifier : IntegralDefuzzifier {
        public override double Defuzzify(Term term, double min, double max) {
            double stepSize = (max - min) / this.samplingRate;
            double largestMembership = 0.0;
            double largestVariableValue = 0.0;
            for (int sample = samplingRate - 1; sample >= 0; --sample) {
                double variableValue = min + (sample + 0.5) * stepSize;
                double termMembership = term.Membership(variableValue);
                if (termMembership > largestMembership) {
                    largestMembership = termMembership;
                    largestVariableValue = variableValue;
                }
            }
            return largestVariableValue;
        }
    }
}
