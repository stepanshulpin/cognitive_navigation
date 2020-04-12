using System;
using System.Collections.Generic;
using AI.FuzzyLogic.Terms;
using UnityEngine;

namespace AI.FuzzyLogic.FuzzyInference {
    public class MamdaniFuzzyInference : FuzzyInferenceSystem {
        public MamdaniFuzzyInference(TNorm conjuction, SNorm disjunction,
            TNorm implication, SNorm aggregation, IntegralDefuzzifier defuzzifier) : base(conjuction, disjunction) {
            this.implication = implication;
            this.aggregation = aggregation;
            this.defuzzifier = defuzzifier;
            activatedRules = new List<Rule>();
        }

        public override Dictionary<string, double> Process(List<Rule> rules,
            Dictionary<string, LinguisticVariable> inputVariables, Dictionary<string, LinguisticVariable> outputVariables) {
            activatedRules.Clear();
            foreach (Rule rule in rules) {
                if (rule.Activate(conjuction, disjunction) > 0.0) {
                    Debug.Log(string.Format("Activated {0} {1}", rule.ToString(), rule.ActivationDegree));
                    activatedRules.Add(rule);
                }
            }
            Dictionary<string, AggregatedTerm> fuzzyOutput = new Dictionary<string, AggregatedTerm>();
            foreach (Rule rule in activatedRules) {
                AggregatedTerm aggregatedOutputTerm;
                foreach (Statement statement in rule.Consequent.Statements) {
                    if (fuzzyOutput.ContainsKey(statement.Variable.Name)) {
                        aggregatedOutputTerm = fuzzyOutput[statement.Variable.Name];
                    } else {
                        aggregatedOutputTerm = new AggregatedTerm("", aggregation);
                        fuzzyOutput.Add(statement.Variable.Name, aggregatedOutputTerm);
                    }
                    Term statementTerm = statement.Variable.GetTerm(statement.Term);
                    int quantifiersCount = statement.Quantifiers.Count - 1;
                    double activationDegree = rule.ActivationDegree;
                    for (int i = quantifiersCount; i >= 0; i--) {
                        activationDegree = statement.Quantifiers[i].Apply(activationDegree);
                    }
                    aggregatedOutputTerm.AddTerm(new ActivatedTerm("", statementTerm, activationDegree, implication));
                }
            }
            Dictionary<string, double> output = new Dictionary<string, double>();
            foreach (var outputPair in fuzzyOutput) {
                LinguisticVariable outputVariable = outputVariables[outputPair.Key];
                output.Add(outputVariable.Name, defuzzifier.Defuzzify(outputPair.Value, outputVariable.Min, outputVariable.Max));
            }
            return output;
        }

        private TNorm implication;

        private SNorm aggregation;

        private List<Rule> activatedRules;

        private IntegralDefuzzifier defuzzifier;
    }
}
