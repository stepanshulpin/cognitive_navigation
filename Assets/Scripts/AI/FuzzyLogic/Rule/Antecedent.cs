using System.Collections.Generic;
using System.Text;

namespace AI.FuzzyLogic {
    public class Antecedent {
        public double ActivationDegree {
            get {
                return activationDegree;
            }
        }

        public Antecedent() {
            statements = new List<Statement>();
            connections = new List<Connection>();
            activationDegree = 0.0;
        }

        public Antecedent(List<Statement> statements, List<Connection> connections) {
            this.statements = statements;
            this.connections = connections;
            activationDegree = 0.0;
        }

        public void AddStatement(Statement statement) {
            statements.Add(statement);
        }

        public void AddConnection(Connection connection) {
            connections.Add(connection);
        }

        public void Activate(TNorm conjuction, SNorm disjunction) {
            activationDegree = statements[0].CalculateProbability();
            if (statements.Count > 1) {
                int statementsCount = statements.Count - 1;
                for (int i = 1; i <= statementsCount; i++) {
                    switch (connections[i - 1]) {
                        case Connection.And:
                            activationDegree = conjuction.Compute(activationDegree, statements[i].CalculateProbability());
                            break;
                        case Connection.Or:
                            activationDegree = disjunction.Compute(activationDegree, statements[i].CalculateProbability());
                            break;
                    }
                }
            }
        }

        public override string ToString() {
            StringBuilder stringBuilder = new StringBuilder();
            if (statements.Count > 0) {
                stringBuilder.Append(statements[0]);
            }
            for (int i = 1; i < statements.Count; ++i) {
                stringBuilder.AppendFormat(" {0} {1}", connections[i - 1].ToString(), statements[i].ToString());
            }
            return stringBuilder.ToString();
        }

        private List<Statement> statements;

        private List<Connection> connections;

        private double activationDegree;
    }
}
