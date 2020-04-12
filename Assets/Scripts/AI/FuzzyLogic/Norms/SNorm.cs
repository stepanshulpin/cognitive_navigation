namespace AI.FuzzyLogic {
    public abstract class SNorm : INorm {
        public NormType Type {
            get {
                return NormType.SNorm;
            }
        }

        public abstract double Compute(double left, double right);
    }
}
