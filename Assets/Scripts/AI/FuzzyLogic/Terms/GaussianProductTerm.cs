using System;
using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms {
    public class GaussianProductTerm : Term {
        public GaussianProductTerm(string name, double meanA, double stdA, double meanB, double stdB) : base(name) {
            this.meanA = meanA;
            this.stdA = stdA;
            this.meanB = meanB;
            this.stdB = stdB;
        }

        public override double Membership(double x) {
            double membershipA = 1.0f;
            double membershipB = 2.0f;
            if (x < meanA) {
                membershipA = Math.Exp(-(x - meanA) * (x - meanA) / (2 * stdA * stdA));
            }
            if (x > meanB) {
                membershipB = Math.Exp(-(x - meanB) * (x - meanB) / (2 * stdB * stdB));
            }
            return membershipA * membershipB;
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
            return Terms.TermType.GaussianProduct;
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

        private double meanA;

        private double stdA;

        private double meanB;

        private double stdB;
    }
}
