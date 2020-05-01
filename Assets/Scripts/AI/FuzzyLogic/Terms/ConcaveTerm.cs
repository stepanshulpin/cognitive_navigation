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

        private double inflection;

        private double end;
    }
}
