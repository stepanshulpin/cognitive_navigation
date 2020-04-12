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
            List<double> parameters = new List<double>();
            parameters.Add(meanA);
            parameters.Add(stdA);
            parameters.Add(meanB);
            parameters.Add(stdB);
            return parameters;
        }

        public override void Update(List<double> parameters)
        {
            if (parameters.Count != 4)
            {
                throw new ArgumentException("Invalid parameters size");
            }
            meanA = parameters[0];
            stdA = parameters[1];
            meanB = parameters[2];
            stdB = parameters[3];
        }

        public override TermType TermType()
        {
            return Terms.TermType.GaussianProduct;
        }


        private double meanA;

        private double stdA;

        private double meanB;

        private double stdB;
    }
}
