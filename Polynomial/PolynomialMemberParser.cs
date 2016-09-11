using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polynomial {
    class PolynomialMemberParser {
        public PolynomialMember Parse(StringBuilder term) {
            if (term.Length == 0) {
                throw new PolynomialParseException("Fail to parse empty term");
            }

            double coefficient = 0D;
            int exponent = 0;

            int signMultiplier;
            int startIndex;
            switch (term[0]) {
                case '+':
                    signMultiplier = 1;
                    startIndex = 1;
                    break;
                case '-':
                    signMultiplier = -1;
                    startIndex = 1;
                    break;
                default:
                    signMultiplier = 1;
                    startIndex = 0;
                    break;
            }

            var coefficientBuilder = new StringBuilder();

            var index = startIndex;
            while (index < term.Length) {
                var c = term[index];
                if (char.IsDigit(c) ||
                    c == '.') {
                    coefficientBuilder.Append(c);
                    index++;
                } else {
                    break;
                }
            }
            if (coefficientBuilder.Length > 0) {
                if (!double.TryParse(coefficientBuilder.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out coefficient)) {
                    throw new PolynomialParseException($"Fail to parse term coefficient in \'{coefficientBuilder}\' of term: \'{term}\'");
                }
            } else {
                coefficient = 1D;
            }

            var variable = new StringBuilder();
            while (index < term.Length) {
                var c = term[index];
                index++;
                if (c == '^') {
                    break;
                }
                variable.Append(c);
            }

            if (variable.Length > 0) {
                var exponentBuilder = new StringBuilder();
                while (index < term.Length) {
                    var c = term[index];
                    index++;
                    exponentBuilder.Append(c);
                }
                if (exponentBuilder.Length == 0) {
                    exponent = 1;
                } else {
                    if (!int.TryParse(exponentBuilder.ToString(), out exponent)) {
                        throw new PolynomialParseException($"Fail to parse term exponent in \'{exponentBuilder}\' of term: \'{term}\'");
                    }
                }
            }

            return new PolynomialMember(variable.ToString(), coefficient * signMultiplier, exponent);
        }
    }
}
