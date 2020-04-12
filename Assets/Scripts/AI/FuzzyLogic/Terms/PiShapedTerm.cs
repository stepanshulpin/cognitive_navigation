﻿using System;
using System.Collections.Generic;

namespace AI.FuzzyLogic.Terms {
    public class PiShapedTerm : Term {
        public PiShapedTerm(string name, double a, double b, double c, double d) : base(name) {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        public override double Membership(double x) {
            if (x <= a || x >= d) {
                return 0;
            }
            if (a <= x && x <= 0.5 * (a + b)) {
                return 2 * Math.Pow((x - a) / (b -a), 2.0);
            }
            if (0.5 * (a + b) <= x && x <= b) {
                return 1.0 - 2 * Math.Pow((x - b) / (b - a), 2.0);
            }
            if (b <= x && x <= c) {
                return 1.0;
            }
            if (c <= x && x <= 0.5 * (c + d)) {
                return 1.0 - 2 * Math.Pow((x - c) / (d - c), 2.0);
            }
            if (0.5 * (c + d) <= x && x <= d) {
                return 2 * Math.Pow((x - d) / (d - c), 2.0);
            }
            return 0.0;
        }

        public override List<double> GetGenericParameters()
        {
            List<double> parameters = new List<double>();
            parameters.Add(a);
            parameters.Add(b);
            parameters.Add(c);
            parameters.Add(d);
            return parameters;
        }

        public override void Update(List<double> parameters)
        {
            if (parameters.Count != 4)
            {
                throw new ArgumentException("Invalid parameters size");
            }
            a = parameters[0];
            b = parameters[1];
            c = parameters[2];
            d = parameters[3];
        }

        public override TermType TermType()
        {
            return Terms.TermType.PiShaped;
        }

        private double a;

        private double b;

        private double c;

        private double d;
    }
}
