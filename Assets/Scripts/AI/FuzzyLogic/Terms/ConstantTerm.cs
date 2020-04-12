using System;
using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms {
    public class ConstantTerm : Term {
        public ConstantTerm(string name, double value) : base(name) {
            this.value = value;
        }

        public override double Membership(double x) {
            return value;
        }

        public override TermType TermType()
        {
            return Terms.TermType.Constant;
        }

        public override List<double> GetGenericParameters()
        {
            List<double> parameters = new List<double>();
            parameters.Add(value);
            return parameters;
        }

        public override void Update(List<double> parameters)
        {
            if (parameters.Count != 1)
            {
                throw new ArgumentException("Invalid parameters size");
            }
            value = parameters[0];
        }

        private double value;
    }
}
