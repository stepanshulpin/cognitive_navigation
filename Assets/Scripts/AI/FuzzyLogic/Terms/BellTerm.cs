using System;
using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms {
    public class BellTerm : Term {
        public BellTerm(string name, double center, double width, double slope) : base(name) {
            this.center = center;
            this.width = width;
            this.slope = slope;
        }

        public BellTerm(string name) : base(name)
        {
            center = 0;
            width = 0;
            slope = 0;
        }

        public override double Membership(double x) {
            return 1.0 / (1 + Math.Pow(Math.Abs((x - center) / width), 2 * slope));
        }

        public override List<double> GetGenericParameters()
        {
            List<double> parameters = new List<double>();
            double lw = -width + center;
            double ls = -width * Math.Pow(0.1 / 0.9, 1 / (2 * slope)) + center;
            double rs = 2 * center - ls;
            double rw = 2 * center - lw;
            parameters.Add(lw);
            parameters.Add(ls);
            parameters.Add(rs);
            parameters.Add(rw);
            return parameters;
        }

        public override void Update(List<double> parameters)
        {
            double lw = parameters[0];
            double ls = parameters[1];
            double rs = parameters[2];
            double rw = parameters[3];
            center = 0.5 * (rs + ls);
            width = center - lw;
            slope = 2 * width / (ls - lw);
        }

        public override TermType TermType()
        {
            return Terms.TermType.Bell;
        }

        public override int Size()
        {
            return 3;
        }

        public override Term Clone()
        {
            return new BellTerm(name, center, width, slope);
        }

        public override void SetValues(double[] values)
        {
            width = values[0];
            slope = values[1];
            center = values[2];
        }

        public override double[] GetValues()
        {
            double[] values = new double[Size()];            
            values[0] = width;
            values[1] = slope;
            values[2] = center;
            return values;
         }

        private double center;

        private double width;//a

        private double slope;//b
    }
}
