namespace AI.FuzzyLogic {
    public abstract class TNorm : INorm {
        public NormType Type {
            get {
                return NormType.TNorm;
            }
        }

        public abstract double Compute(double left, double right);
    }
}
