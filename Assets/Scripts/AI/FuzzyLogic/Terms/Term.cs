using System;
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

        public Term(string name) {
            this.name = name;
        }

        public abstract int Size();
        public abstract TermType TermType();
        public abstract double Membership(double x);
        public abstract List<double> GetGenericParameters();
        public abstract void Update(List<double> parameters);
        public abstract Term Clone();

        protected string name;

        public abstract void SetValues(double[] values);

        public abstract double[] GetValues();
    }
}
