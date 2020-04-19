using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AI.FuzzyLogic.FuzzyInference;
using UnityEngine;

namespace AI.FuzzyLogic {
    public class Engine {
        public List<string> InputVariables {
            get {
                return inputVariables.Keys.ToList();
            }
        }

        public List<string> OutputVariables {
            get {
                return outputVariables.Keys.ToList();
            }
        }

        public Engine(IFuzzyInferenceSystem fuzzyInferenceSystem) {
            this.fuzzyInferenceSystem = fuzzyInferenceSystem;
            inputVariables = new Dictionary<string, LinguisticVariable>();
            outputVariables = new Dictionary<string, LinguisticVariable>();
            quantifiers = new Dictionary<string, IQuantifier>();
            LoadDefaultQuantifiers();
            parser = new RuleParser(quantifiers);
            rules = new List<Rule>();
        }

        public void RegisterInputVariable(LinguisticVariable inputVariable) {
            inputVariables.Add(inputVariable.Name, inputVariable);
        }

        public void RegisterOutputVariable(LinguisticVariable outputVariable) {
            outputVariables.Add(outputVariable.Name, outputVariable);
        }

        public void ExcludeInputVariable(string variable) {
            inputVariables.Remove(variable);
        }

        public void ExcluseOutputVariable(string variable) {
            outputVariables.Remove(variable);
        }

        public void RegisterRule(string rule) {
            Rule parsedRule = parser.Parse(rule, inputVariables, outputVariables);
            rules.Add(parsedRule);
        }

        public void RegisterRule(Rule rule) {
            rules.Add(rule);
        }

        public void ExcludeRule(int index) {
            rules.RemoveAt(index);
        }

        public void ExcludeRule(Rule rule) {
            rules.Remove(rule);
        }

        public Dictionary<string, double> Process(Dictionary<string, double> input) {
            //Debug.Log("Processing input");
            foreach (var inputPair in input) {
                //Debug.Log(string.Format("{0} : {1}", inputPair.Key, inputPair.Value));
            }
            //Debug.Log("Fuzzy input");
            Fuzzify(input);
            foreach (var variabe in inputVariables.Values) {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendFormat("{0} is ", variabe.Name);
                foreach (var terms in variabe.Fuzzy) {
                    stringBuilder.AppendFormat("{0} {1} ", terms.Value, terms.Key);
                }
                //Debug.Log(stringBuilder.ToString());
            }
            return fuzzyInferenceSystem.Process(rules, inputVariables, outputVariables);
        }

        private Dictionary<string, LinguisticVariable> inputVariables;

        private Dictionary<string, LinguisticVariable> outputVariables;

        private Dictionary<string, IQuantifier> quantifiers;

        private List<Rule> rules;

        private IFuzzyInferenceSystem fuzzyInferenceSystem;

        private RuleParser parser;

        private void Fuzzify(Dictionary<string, double> input) {
            foreach (LinguisticVariable inputVariable in inputVariables.Values) {
                inputVariable.Value = input[inputVariable.Name];
                inputVariable.Fuzzify();
            }
        }

        private void LoadDefaultQuantifiers() {
            IQuantifier quantifier = new Not();
            quantifiers.Add(quantifier.Name, quantifier);
            quantifier = new Seldom();
            quantifiers.Add(quantifier.Name, quantifier);
            quantifier = new Somewhat();
            quantifiers.Add(quantifier.Name, quantifier);
            quantifier = new Very();
            quantifiers.Add(quantifier.Name, quantifier);
            quantifier = new Extremely();
            quantifiers.Add(quantifier.Name, quantifier);
            quantifier = new Any();
            quantifiers.Add(quantifier.Name, quantifier);
        }


    }
}
