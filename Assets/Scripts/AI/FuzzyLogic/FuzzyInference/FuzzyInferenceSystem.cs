using System.Collections.Generic;

namespace AI.FuzzyLogic.FuzzyInference {
    public abstract class FuzzyInferenceSystem : IFuzzyInferenceSystem {
        public FuzzyInferenceSystem(TNorm conjuction, SNorm disjunction) {
            this.conjuction = conjuction;
            this.disjunction = disjunction;
        }

        public abstract Dictionary<string, double> Process(List<Rule> rules,
            Dictionary<string, LinguisticVariable> inputVariables, Dictionary<string, LinguisticVariable> outputVariables);

        protected TNorm conjuction;

        protected SNorm disjunction;
    }
}
