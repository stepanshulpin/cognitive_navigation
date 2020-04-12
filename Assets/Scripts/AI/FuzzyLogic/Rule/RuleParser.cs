using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AI.FuzzyLogic {
    public class RuleParser {
        public RuleParser(Dictionary<string, IQuantifier> quantifiers) {
            this.quantifiers = quantifiers;
            keywords = new Keywords();
        }

        public Rule Parse(string rule, Dictionary<string, LinguisticVariable> inputVariables,
            Dictionary<string, LinguisticVariable> outputVariables) {
            Validate(rule);
            string[] ruleParts = rule.Split(new string[] { keywords.Then }, StringSplitOptions.RemoveEmptyEntries);
            if (ruleParts.Length != 2) {
                throw new FormatException("Wrong rule format, should be: IF antecedent THEN consequent");
            }
            if (!DoesConsequentContainOnlyConjuctions(ruleParts[1])) {
                throw new FormatException("Consequent shouldn't contains OR keywords");
            }
            Antecedent antecedent = ParseAntecedent(ruleParts[0], inputVariables);
            Consequent consequent = ParseConsequent(ruleParts[1], outputVariables);
            Rule parsedRule = new Rule(antecedent, consequent);
            return parsedRule;
        }

        private Antecedent ParseAntecedent(string antecedent, Dictionary<string, LinguisticVariable> inputVariables) {
            antecedent = antecedent.Replace(keywords.If, "");
            string[] tokens = antecedent.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length < 1) {
                throw new FormatException("Antecedent couldn't be empty");
            }
            List<Statement> statements;
            List<Connection> connections;
            Parse(tokens, inputVariables, out statements, out connections);
            Antecedent parsedAntecedent = new Antecedent(statements, connections);
            return parsedAntecedent;
        }

        private Consequent ParseConsequent(string consequent, Dictionary<string, LinguisticVariable> outputVariables) {
            List<Statement> statements;
            List<Connection> connections;
            string[] tokens = consequent.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
            Parse(tokens, outputVariables, out statements, out connections);
            Consequent parsedConsequent = new Consequent(statements);
            return parsedConsequent;
        }

        private void Validate(string rule) {
            if (IsEmpty(rule)) {
                throw new ArgumentNullException("Rule can't be null");
            }
            if (!DoesStartWithIf(rule)) {
                throw new FormatException("Rule must starts with IF keyword");
            }
            if (!DoesContainOnlyOneIf(rule)) {
                throw new FormatException("Rule must contains only one IF keyword");
            }
            if (!DoesContainOnlyOneImplication(rule)) {
                throw new FormatException("Wrong rule format, should be: IF antecedent THEN consequent");
            }
        }

        private bool IsEmpty(string rule) {
            return string.IsNullOrEmpty(rule);
        }

        private bool DoesStartWithIf(string rule) {
            return rule.StartsWith(string.Format("{0}", keywords.If));
        }

        private bool DoesContainOnlyOneIf(string rule) {
            return Regex.Matches(rule, string.Format(@"{0}\b", keywords.If)).Count == 1;
        }

        private bool DoesContainOnlyOneImplication(string rule) {
            return Regex.Matches(rule, string.Format(@"\b{0}\b", keywords.Then)).Count == 1;
        }

        private bool DoesConsequentContainOnlyConjuctions(string consequent) {
            return Regex.Matches(consequent, string.Format(@"\b{0}\b", keywords.Or)).Count == 0;
        }

        private void Parse(string[] tokens, Dictionary<string, LinguisticVariable> variables, out List<Statement> statements, out List<Connection> connections) {
            statements = new List<Statement>();
            connections = new List<Connection>();
            Statement statement;
            if (variables.ContainsKey(tokens[0])) {
                statement = new Statement(variables[tokens[0]]);
                statements.Add(statement);
            } else {
                throw new FormatException(string.Format("Variable name was expected, but got {0}", tokens[0]));
            }
            ParserState state = ParserState.IsKeyword;
            for (int i = 1; i < tokens.Length; i++) {
                string token = tokens[i];
                switch (state) {
                    case ParserState.IsKeyword:
                        if (!token.Equals(keywords.Is)) {
                            throw new FormatException(string.Format("Keyword IS was expected, but got {0}", token));
                        }
                        state = ParserState.QuantifierOrTerm;
                        break;
                    case ParserState.QuantifierOrTerm:
                        if (quantifiers.ContainsKey(token)) {
                            statement.AddQuantifier(quantifiers[token]);
                        } else if (statement.Variable.HasTerm(token)) {
                            statement.Term = token;
                            state = ParserState.Norm;
                        } else {
                            throw new FormatException(string.Format("Term or quantifier was expected, but got {0}", token));
                        }
                        break;
                    case ParserState.Norm:
                        if (token.Equals(keywords.And)) {
                            connections.Add(Connection.And);
                        } else if (token.Equals(keywords.Or)) {
                            connections.Add(Connection.Or);
                        } else {
                            throw new FormatException(string.Format("Keyword AND or OR was expected, but got {0}", token));
                        }
                        state = ParserState.VariableOrQuantifierOrTerm;
                        break;
                    case ParserState.VariableOrQuantifierOrTerm:
                        if (variables.ContainsKey(token)) {
                            statement = new Statement(variables[token]);
                            statements.Add(statement);
                            state = ParserState.IsKeyword;
                        } else if (quantifiers.ContainsKey(token)) {
                            statement = new Statement(statement.Variable);
                            statement.AddQuantifier(quantifiers[token]);
                            statements.Add(statement);
                            state = ParserState.QuantifierOrTerm;
                        } else if (statement.Variable.HasTerm(token)) {
                            statement = new Statement(statement.Variable);
                            statement.Term = token;
                            statements.Add(statement);
                            state = ParserState.Norm;
                        } else {
                            throw new FormatException(string.Format("Variable or quantifier or term was expected, but got {0}", token));
                        }
                        break;
                    default:
                        throw new FormatException(string.Format("Unhandled parser state {0} for token {1}", state, token));
                }
            }
        }

        private Dictionary<string, IQuantifier> quantifiers;

        private Keywords keywords;

        private enum ParserState {
            IsKeyword,
            QuantifierOrTerm,
            Norm,
            VariableOrQuantifierOrTerm
        }

        private class Keywords {
            public string If {
                get {
                    return "IF";
                }
            }

            public string Then {
                get {
                    return "THEN";
                }
            }

            public string Is {
                get {
                    return "IS";
                }
            }

            public string And {
                get {
                    return "AND";
                }
            }

            public string Or {
                get {
                    return "OR";
                }
            }
        }
    }
}
