using System;
using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms {
    public class RLinearTerm : Term {
        public RLinearTerm(string name, double start, double end) : base(name) {
            this.start = start;
            this.end = end;
        }

        public override double Membership(double x) {
            if (x < start) {
                return 0.0;
            }
            if (x > end) {
                return 1.0;
            }
            return (x - start) / (end - start);
        }

        public override List<double> GetGenericParameters()
        {
            throw new NotImplementedException();
        }

        public override void Update(List<double> parameters)
        {
            throw new NotImplementedException();
        }

        public override TermType TermType()
        {
            return Terms.TermType.RLinear;
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

        private double start;

        private double end;
    }
}
