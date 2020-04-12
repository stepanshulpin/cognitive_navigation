using AI.FuzzyLogic.Terms;

namespace AI.FuzzyLogic {
    public class CentroidDefuzzifier : IntegralDefuzzifier {
        public override double Defuzzify(Term term, double min, double max) {
            double stepSize = (max - min) / samplingRate;
            double centroid = 0.0;
            double area = 0.0;
            for (int sample = 0; sample < samplingRate; ++sample) {
                double variableValue = min + (sample + 0.5) * stepSize;
                double termMembership = term.Membership(variableValue);
                centroid += variableValue * termMembership;
                area += termMembership;
            }
            return centroid / area;
        }
    }
}
