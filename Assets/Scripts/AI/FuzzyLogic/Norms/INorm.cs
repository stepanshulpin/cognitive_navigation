namespace AI.FuzzyLogic {
    public enum NormType {
        TNorm,
        SNorm
    }

    public interface INorm {
        NormType Type {
            get;
        }

        double Compute(double left, double right);
    }
}
