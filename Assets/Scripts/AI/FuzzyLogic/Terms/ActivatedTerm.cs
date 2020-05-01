using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms {
    public class ActivatedTerm : Term {
        public ActivatedTerm(string name, Term term, double activationDegree, TNorm implication) : base(name) {
            this.term = term;
            this.activationDegree = activationDegree;
            this.implication = implication;
        }

        public override double Membership(double x) {
            return implication.Compute(term.Membership(x), activationDegree);
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

        private Term term;

        private double activationDegree;

        private TNorm implication;
    }
}
