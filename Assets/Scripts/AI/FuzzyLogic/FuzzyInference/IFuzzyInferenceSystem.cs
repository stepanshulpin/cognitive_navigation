using System.Collections.Generic;

namespace AI.FuzzyLogic.FuzzyInference {
    public interface IFuzzyInferenceSystem {
        Dictionary<string, double> Process(List<Rule> rules,
            Dictionary<string, LinguisticVariable> inputVariables, Dictionary<string, LinguisticVariable> outputVariables);
    }
}
