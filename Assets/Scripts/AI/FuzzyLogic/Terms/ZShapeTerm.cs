using System;
using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms {
    public class ZShapeTerm : Term {
        public ZShapeTerm(string name, double start, double end) : base(name) {
            this.start = start;
            this.end = end;
        }

        public ZShapeTerm(string name) : base(name)
        {
            start = 0;
            end = 0;
        }

        public override double Membership(double x) {
            if (x <= start) {
                return 1.0;
            }
            if (x <= 0.5 * (start + end)) {
                return 1.0 - 2 * Math.Pow((x - start) / (end - start), 2.0);
            }
            if (x < this.end) {
                return 2 * Math.Pow((x - end) / (end - start), 2.0);
            }
            return 0.0;
        }

        public override List<double> GetGenericParameters()
        {
            List<double> parameters = new List<double>();
            double lw;
            double ls = end;
            double rs = end;
            double rw;
            if (start < end)
            {
                lw = start;
                rw = 2 * end - start;
            } else
            {
                lw = 2 * end - start;
                rw = start;
            }
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
            start = rs;
            end = 2 * rw - rs;
        }

        public override TermType TermType()
        {
            return Terms.TermType.ZShape;
        }

        public override int Size()
        {
            return 2;
        }

        public override Term Clone()
        {
            return new ZShapeTerm(name, start, end);
        }

        public override void SetValues(double[] values)
        {
            start = values[0];
            end = values[1];
        }

        public override double[] GetValues()
        {
            double[] values = new double[Size()];
            values[0] = start;
            values[1] = end;
            return values;
        }

        private double start;

        private double end;
    }
}
