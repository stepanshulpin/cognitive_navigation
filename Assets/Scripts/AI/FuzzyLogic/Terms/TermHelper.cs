using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.FuzzyLogic.Terms
{
    public static class TermHelper
    {
        public static int TERM_TYPES_COUNT = 7;

        public static TermType getTermType(int index)
        {
            switch(index) {
                case 0: return TermType.Triangular;                    
                case 1: return TermType.Trapezodial;
                case 2: return TermType.Bell;
                case 3: return TermType.Gaussian;
                case 4: return TermType.Sigmoidal;
                case 5: return TermType.SShape;
                case 6: return TermType.ZShape;
            }
            throw new Exception("Term type for index doesn't exist");
        }

        public static Term instantiate(TermType termType, string name)
        {
            switch(termType)
            {
                case TermType.Triangular: return new TriangularTerm(name);
                case TermType.Trapezodial: return new TrapezoidalTerm(name);
                case TermType.Bell: return new BellTerm(name);
                case TermType.Gaussian: return new GaussianTerm(name);
                case TermType.Sigmoidal: return new SigmoidalTerm(name);
                case TermType.SShape: return new SShapeTerm(name);
                case TermType.ZShape: return new ZShapeTerm(name);
            }
            throw new Exception("Instantiate doesn't implement for this type");
        }
    }
}
