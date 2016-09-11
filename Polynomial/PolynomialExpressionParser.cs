using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Polynomial {
    class PolynomialExpressionParser {
        private readonly PolynomialMemberParser _memberParser;
        public PolynomialExpressionParser() {
            _memberParser = new PolynomialMemberParser();
        }

        public PolynomialExpression Parse(string source) {

            new ParenthesisValidator().Validate(source);

            var leftSide = new StringBuilder();
            var rightSide = new StringBuilder();

            _splitToSidesWithoutSpaces(source, leftSide, rightSide);

            var leftMembers = _parseMembers(leftSide);

            var rightMembers = _parseMembers(rightSide);

            var expression = new PolynomialExpression();

            foreach (var leftMember in leftMembers) {
                expression.AddMember(leftMember);
            }
            foreach (var rightMember in rightMembers) {
                expression.AddMember(new PolynomialMember(rightMember.Variable, rightMember.Coefficient*-1, rightMember.Exponent));
            }

            return expression;
        }

        private List<PolynomialMember> _parseMembers(StringBuilder sb) {
            var terms = _splitToTerms(sb);
            var members  = new List<PolynomialMember>();
            foreach (var term in terms) {
                if (_isTermWithParenthesis(term)) {
                    members.AddRange(_revealParenthesis(term));
                } else {
                    members.Add(_memberParser.Parse(term));
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

        private IEnumerable<PolynomialMember> _revealParenthesis(StringBuilder term) {
            var parenthesisContent = _getParenthesisContent(term);
            var members = _parseMembers(parenthesisContent);
            Func<IEnumerable<PolynomialMember>, IEnumerable<PolynomialMember>> functor = _defineRevealFunctor(term);
            return functor(members);
        }

        private Func<IEnumerable<PolynomialMember>, IEnumerable<PolynomialMember>> _defineRevealFunctor(StringBuilder term) {

            Func<IEnumerable<PolynomialMember>, IEnumerable<PolynomialMember>> multiplierFunctor = null;
            Func<IEnumerable<PolynomialMember>, IEnumerable<PolynomialMember>> divideFunctor = null;

            var expressionBefor = new StringBuilder();
            for (int i = 0; i < term.Length; i++) {
                var c = term[i];
                if (c == '*') {
                    continue;
                }
                if (c == '(') {
                    break;
                }
                expressionBefor.Append(c);
            }

            if (expressionBefor.Length > 0) {

                var signMultiplier = 1;
                if (expressionBefor[0] == '+') {
                    expressionBefor.Remove(0, 1);
                } else if (expressionBefor[0] == '-') {
                    signMultiplier = -1;
                    expressionBefor.Remove(0, 1);
                }

                double multiplier;
                if (expressionBefor.Length == 0) {
                    multiplier = 1;
                } else {
                    if (!double.TryParse(expressionBefor.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out multiplier)) {
                        throw new PolynomialParseException($"Fail to parse expression before parenthesis \'{expressionBefor}\' in term \'{term}\'");
                    }
                }

                multiplierFunctor = members => members.Select(_ => new PolynomialMember(_.Variable, _.Coefficient*multiplier*signMultiplier, _.Exponent)).ToList();
            }

            var lastParenthesisIndex = term.Length - 1;
            while (lastParenthesisIndex >= 0) {
                if (term[lastParenthesisIndex] == ')') {
                    break;
                }
                lastParenthesisIndex--;
            }

            if (lastParenthesisIndex < term.Length - 1) {
                var expressionAfter = new StringBuilder();
                for (int i = lastParenthesisIndex + 1; i < term.Length; i++) {
                    var c = term[i];
                    if (c == '/') continue;
                    expressionAfter.Append(term[i]);
                }

                double divider;
                if (!double.TryParse(expressionAfter.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out divider)) {
                    throw new PolynomialParseException($"Fail to parse expression after parenthesis \'{expressionAfter}\' in term \'{term}\'");
                }
                divideFunctor =
                    members => members.Select(_ => new PolynomialMember(_.Variable, _.Coefficient/divider, _.Exponent)).ToList();
            }

            return members => {
                if (multiplierFunctor != null) {
                    members = multiplierFunctor(members);
                }
                if (divideFunctor != null) {
                    members = divideFunctor(members);
                }
                return members;
            };
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

    class ParenthesisValidator {
        public void Validate(string source) {
            var leftParenthesis = 0;
            var rightParenthesis = 0;

            for (int i = 0; i < source.Length; i++) {
                var c = source[i];
                if (c == '(') leftParenthesis++;
                if (c == ')') rightParenthesis++;
            }

            if (leftParenthesis != rightParenthesis) throw new PolynomialParseException("Left parenthesis count not equal to right parenthesis count.");
        }
    }
}
