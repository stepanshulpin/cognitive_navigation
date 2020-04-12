namespace AI.FuzzyLogic {
    public enum QuantifierOrder {
        Not,
        Seldom,
        Somewhat,
        Very,
        Extremely,
        Any
    }

    public interface IQuantifier {
        QuantifierOrder Order {
            get;
        }

        string Name {
            get;
        }

        double Apply(double membership);
    }
}
