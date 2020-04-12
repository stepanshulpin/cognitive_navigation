using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms {

    public enum TermType
    {
        Bell,
        Concave,
        Constant,
        Discrete,
        Gaussian,
        GaussianProduct,
        LLinear,
        PiShaped,
        Rectangular,
        RLinear,
        Sigmoidal,
        SShape,
        Trapezodial,
        Triangular,
        ZShape,
    }

    public abstract class Term {
        public string Name {
            get {
                return name;
            }
        }
        public abstract TermType TermType();


        public Term(string name) {
            this.name = name;
        }

        public abstract double Membership(double x);
        public abstract List<double> GetGenericParameters();
        public abstract void Update(List<double> parameters);


        private string name;
    }
}
