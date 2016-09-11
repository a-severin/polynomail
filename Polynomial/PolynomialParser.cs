using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Polynomial {
    class PolynomialParser {
        public PolynomialExpression Parse(string source) {

            var expression = new PolynomialExpression();

            var leftSide = new StringBuilder();
            var rightSide = new StringBuilder();

            _splitToSidesWithoutSpaces(source, leftSide, rightSide);

            var leftMembers = _parseMembers(leftSide);

            var rightMembers = _parseMembers(rightSide);

            return expression;
        }

        private List<PolynomialMember> _parseMembers(StringBuilder sb) {
            var terms = _splitToTerms(sb);
            var members  = new List<PolynomialMember>();
            foreach (var term in terms) {
                if (_isTermWithParenthesis(term)) {
                    members.AddRange(_revealParenthesis(term));
                } else {
                    members.Add(_parseMember(term));
                }
            }
            return members;
        }


        private void _splitToSidesWithoutSpaces(string source, StringBuilder leftSide, StringBuilder rightSide) {
            var sb = leftSide;
            for (int i = 0; i < source.Length; i++) {
                var c = source[i];
                switch (c) {
                    case ' ':
                        continue;
                    case '=':
                        sb = rightSide;
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
        }

        private List<StringBuilder> _splitToTerms(StringBuilder sb) {
            var terms = new List<StringBuilder>();

            var term = new StringBuilder();
            int insideParenthesis = 0;
            for (int i = 0; i < sb.Length; i++) {
                var c = sb[i];
                switch (c) {
                    case '(':
                        insideParenthesis++;
                        break;
                    case ')':
                        insideParenthesis--;
                        break;
                    case '+':
                    case '-':
                        if (i != 0 && insideParenthesis == 0) {
                            terms.Add(term);
                            term = new StringBuilder();
                        }
                        break;
                }
                term.Append(c);
            }
            terms.Add(term);

            return terms;
        }

        private bool _isTermWithParenthesis(StringBuilder sb) {
            for (int i = 0; i < sb.Length; i++) {
                if (sb[i] == '(') return true;
            }
            return false;
        }

        private PolynomialMember _parseMember(StringBuilder term) {

            if (term.Length == 0) {
                throw new PolynomialParseException("Fail to parse empty term");
            }

            double coefficient;
            int exponent;

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
                if (!double.TryParse(coefficientBuilder.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out coefficient )) {
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

            var exponentBuilder = new StringBuilder();
            while (index < term.Length) {
                var c = term[index];
                index++;
                exponentBuilder.Append(c);
            }
            if (exponentBuilder.Length == 0) {
                exponent = 0;
            } else {
                if (!int.TryParse(exponentBuilder.ToString(), out exponent)) {
                    throw new PolynomialParseException($"Fail to parse term exponent in \'{exponentBuilder}\' of term: \'{term}\'");
                }
            }

            return new PolynomialMember(variable.ToString(), coefficient * signMultiplier, exponent);
        }

        private IEnumerable<PolynomialMember> _revealParenthesis(StringBuilder term) {
            // define functor for revealing
            Func<IEnumerable<PolynomialMember>, IEnumerable<PolynomialMember>> functor = _defineRevealFunctor(term);
            var parenthesisContent = _getParenthesisContent(term);
            var members = _parseMembers(parenthesisContent);
            return functor(members);
        }

        private Func<IEnumerable<PolynomialMember>, IEnumerable<PolynomialMember>> _defineRevealFunctor(StringBuilder term) {
            return members => members;
        }

        private StringBuilder _getParenthesisContent(StringBuilder term) {
            int start = 0;

            for (int i = 0; i < term.Length; i++) {
                var c = term[i];
                if (c == '(') {
                    start = i + 1;
                    break;
                }
            }

            int stop = term.Length;
            for (int i = term.Length - 1; i > start; i--) {
                var c = term[i];
                if (c == ')') {
                    stop = i - 1;
                    break;
                }
            }

            var content = new StringBuilder();
            for (int i = start; i <= stop; i++) {
                content.Append(term[i]);
            }

            return content;
        }

    }

    public class PolynomialParseException : ApplicationException {
        public PolynomialParseException(string message) : base(message) {
        }

    }
}
