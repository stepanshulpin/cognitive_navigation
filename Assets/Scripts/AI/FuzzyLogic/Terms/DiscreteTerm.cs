using System;
using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms {
    public class DiscreteTerm : Term {
        public DiscreteTerm(string name, double[] x, double[] y) : base(name) {
            this.x = x;
            this.y = y;
            lastPointIndex = x.Length - 1;
        }

        public override double Membership(double value) {
            if (value < x[0]) {
                return y[0];
            }
            if (value > x[lastPointIndex]) {
                return y[lastPointIndex];
            }
            int lowerBoundIndex = 0;
            while (value > x[lowerBoundIndex] && !(Math.Abs(value - x[lowerBoundIndex]) < double.Epsilon) ) {
                lowerBoundIndex++;
            }
            if (Math.Abs(value - x[lowerBoundIndex]) < double.Epsilon) {
                return y[lowerBoundIndex];
            }
            int upperBoundIndex = lastPointIndex;
            while (value < x[upperBoundIndex]) {
                upperBoundIndex--;
            }
            upperBoundIndex++;
            lowerBoundIndex--;
            return Scale(value, x[lowerBoundIndex], x[upperBoundIndex], y[lowerBoundIndex], y[upperBoundIndex]);
        }

        private double Scale(double x, double xMin, double xMax, double yMin, double yMax) {
            return (yMax - yMin) / (xMax - xMin) * (x - xMin) + yMin;
        }        

        public override TermType TermType()
        {
            return Terms.TermType.Discrete;
        }

        public override List<double> GetGenericParameters()
        {
            throw new NotImplementedException();
        }

        public override void Update(List<double> parameters)
        {
            throw new NotImplementedException();
        }

        private double[] x;

        private double[] y;

        private int lastPointIndex;
    }
}
