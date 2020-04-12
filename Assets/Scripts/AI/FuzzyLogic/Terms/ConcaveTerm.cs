using System;
using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms {
    public class ConcaveTerm : Term {
        public ConcaveTerm(string name, double inflection, double end) : base(name) {
            this.inflection = inflection;
            this.end = end;
        }

        public override double Membership(double x) {
            if (inflection < end) {
                if (x < end) {
                    return (end - inflection) / (2 * end - inflection - x);
                }
            } else {
                if (x > end) {
                    return (inflection - end) / (inflection - 2 * end + x);
                }
            }
            return 1.0;
        }

        public override TermType TermType()
        {
            return Terms.TermType.Concave;
        }

        public override List<double> GetGenericParameters()
        {
            List<double> parameters = new List<double>();
            parameters.Add(inflection);
            parameters.Add(end);
            return parameters;
        }

        public override void Update(List<double> parameters)
        {
            if (parameters.Count != 2)
            {
                throw new ArgumentException("Invalid parameters size");
            }
            inflection = parameters[0];
            end = parameters[1];
        }

        private double inflection;

        private double end;
    }
}
