using System.Collections.Generic;
using System.Text;

namespace AI.FuzzyLogic {
    public class Consequent {
        public List<Statement> Statements {
            get {
                return statements;
            }
        }

        public Consequent() {
            statements = new List<Statement>();
        }

        public Consequent(List<Statement> statements) {
            this.statements = statements;
        }

        public void AddStatement(Statement statement) {
            statements.Add(statement);
        }

        public override string ToString() {
            StringBuilder stringBuilder = new StringBuilder();
            if (statements.Count > 0) {
                stringBuilder.Append(statements[0].ToString());
            }
            for (int i = 1; i < statements.Count; ++i) {
                stringBuilder.AppendFormat(" and {0}", statements[i].ToString());
            }
            return stringBuilder.ToString();
        }

        private List<Statement> statements;
    }
}
