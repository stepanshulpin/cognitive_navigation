using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms {
    public class AggregatedTerm : Term {
        public AggregatedTerm(string name, SNorm aggregation) : base(name) {
            this.aggregation = aggregation;
            terms = new List<Term>();
        }

        public void AddTerm(Term term) {
            terms.Add(term);
        }

        public void Clear() {
            terms.Clear();
        }

        public override double Membership(double x) {
            double membership = 0.0;
            foreach (Term term in terms) {
                membership = aggregation.Compute(membership, term.Membership(x));
            }
            return membership;
        }

        public override TermType TermType()
        {
            throw new System.NotImplementedException();
        }

        public override List<double> GetGenericParameters()
        {
            throw new System.NotImplementedException();
        }

        public override void Update(List<double> parameters)
        {
            throw new System.NotImplementedException();
        }

        public override int Size()
        {
            throw new System.NotImplementedException();
        }

        public override Term Clone()
        {
            throw new System.NotImplementedException();
        }

        public override void SetValues(double[] values)
        {
            throw new System.NotImplementedException();
        }

        public override double[] GetValues()
        {
            throw new System.NotImplementedException();
        }

        private List<Term> terms;

        private SNorm aggregation;
    }
}
