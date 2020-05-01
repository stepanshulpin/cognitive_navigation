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
            throw new NotImplementedException();
        }

        public override void Update(List<double> parameters)
        {
            throw new NotImplementedException();
        }

        public override int Size()
        {
            throw new NotImplementedException();
        }

        public override Term Clone()
        {
            throw new NotImplementedException();
        }

        public override void SetValues(double[] values)
        {
            throw new NotImplementedException();
        }

        public override double[] GetValues()
        {
            throw new NotImplementedException();
        }

        private double value;
    }
}
