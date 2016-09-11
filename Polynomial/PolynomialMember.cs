using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polynomial {
    class PolynomialMember {
        public readonly string Variable;
        public readonly double Coefficient;
        public readonly int Exponent;

        public PolynomialMember(string variable, double coefficient, int exponent) {
            Variable = variable;
            Coefficient = coefficient;
            Exponent = exponent;
        }

#if DEBUG
        public override string ToString() {
            return $"{Coefficient}{Variable}^{Exponent}";
        }
#endif
    }
}
