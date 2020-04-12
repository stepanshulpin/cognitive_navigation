using AI.FuzzyLogic.Terms;

namespace AI.FuzzyLogic {
    public class BisectorDefuzzifier : IntegralDefuzzifier {
        public override double Defuzzify(Term term, double min, double max) {
            double stepSize = (max - min) / samplingRate;
            double leftArea = 0.0;
            double leftBorder = min;
            int stepsFromLeft = 0;
            double rightArea = 0.0;
            double rightBorder = max;
            int stepsFromRight = 0;
            for (int sample = 0; sample < samplingRate; ++sample) {
                if (leftArea < rightArea) {
                    leftBorder = min + (stepsFromLeft + 0.5) * stepSize;
                    leftArea += term.Membership(leftBorder);
                    stepsFromLeft++;
                } else {
                    rightBorder = max - (stepsFromRight + 0.5) * stepSize;
                    rightArea += term.Membership(rightBorder);
                    stepsFromRight++;
                }
            }
            return (leftArea * leftBorder + rightArea * rightBorder) / (leftArea + rightArea);
        }
    }
}
