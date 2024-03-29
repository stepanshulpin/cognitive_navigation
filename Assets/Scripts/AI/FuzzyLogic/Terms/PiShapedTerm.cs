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
            throw new NotImplementedException();
        }

        public override void Update(List<double> parameters)
        {
            throw new NotImplementedException();
        }

        public override TermType TermType()
        {
            return Terms.TermType.PiShaped;
        }

        public override int Size()
        {
            throw new NotImplementedException();
        }

        public override Term Clone()
        {
            throw new NotImplementedException();
        }

        public override void SetValues(double[] values)
        {
            throw new NotImplementedException();
        }

        public override double[] GetValues()
        {
            throw new NotImplementedException();
        }

        private double a;

        private double b;

        private double c;

        private double d;
    }
}
