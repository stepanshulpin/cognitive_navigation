using AI.FuzzyLogic.Terms;

namespace AI.FuzzyLogic {
    public abstract class IntegralDefuzzifier : IDefuzzifier {
        public int SamplingRate {
            get {
                return  samplingRate;
            }
            set {
                samplingRate = value;
            }
        }

        public IntegralDefuzzifier() : this(100) {
        }

        public IntegralDefuzzifier(int samplingRate) {
            this.samplingRate = samplingRate;
        }

        public abstract double Defuzzify(Term term, double min, double max);

        protected int samplingRate;
    }
}
