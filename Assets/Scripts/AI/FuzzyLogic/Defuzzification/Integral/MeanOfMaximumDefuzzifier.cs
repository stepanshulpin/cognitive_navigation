using System;
using AI.FuzzyLogic.Terms;

namespace AI.FuzzyLogic {
    public class MeanOfMaximumDefuzzifier : IntegralDefuzzifier {
        public override double Defuzzify(Term term, double min, double max) {
            double stepSize = (max - min) / samplingRate;
            double largestMembership = 0.0;
            double smallestVariableValue = min;
            double largestVariableValue = min;
            bool isMaximumContinuous = true;
            for (int sample = 0; sample < samplingRate; ++sample) {
                double variableValue = min + (sample + 0.5) * stepSize;
                double termMembership = term.Membership(variableValue);
                if (isMaximumContinuous && Math.Abs(termMembership - largestMembership) < 1e-8) {
                    largestVariableValue = variableValue;
                } else if (termMembership > largestMembership) {
                    largestMembership = termMembership;
                    smallestVariableValue = variableValue;
                    largestVariableValue = variableValue;
                    isMaximumContinuous = true;
                } else {
                    isMaximumContinuous = false;
                }
            }
            return 0.5 * (smallestVariableValue + largestVariableValue);
        }
    }
}
