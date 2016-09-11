using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Polynomial {
    internal class PolynomialExpression {
        private readonly List<PolynomialMember> Members = new List<PolynomialMember>();

        public void AddMember(PolynomialMember member) {
            Members.Add(member);
        }

        public PolynomialExpression CollectAllTerms() {
            var collectedPolynomialExpresstion = new PolynomialExpression();
            foreach (var memberGroupByVariable in Members.GroupBy(_ => _.Variable)) {
                foreach (var memberGroupByExponent in memberGroupByVariable.GroupBy(_ => _.Exponent)) {
                    var coefficient = memberGroupByExponent.Sum(_ => _.Coefficient);
                    if (Math.Abs(coefficient) <= double.Epsilon) {
                        continue;
                    }
                    collectedPolynomialExpresstion.AddMember(new PolynomialMember(memberGroupByVariable.Key, coefficient, memberGroupByExponent.Key));
                }
            }
            return collectedPolynomialExpresstion;
        }

        public string Serialize() {
            var sb = new StringBuilder();

            if (Members.Count == 0) {
                return sb.ToString();
            }

            for (var i = 0; i < Members.Count; i++) {
                var member = Members[i];

                if (i > 0) {
                    if (member.Coefficient > 0) {
                        sb.Append("+ ");
                    }
                }
                if (member.Coefficient < 0) {
                    sb.Append("- ");
                }

                if (Math.Abs(Math.Abs(member.Coefficient) - 1D) > double.Epsilon) {
                    sb.Append(Math.Abs(member.Coefficient).ToString(CultureInfo.InvariantCulture));
                }

                if (member.Exponent > 0) {
                    sb.Append(member.Variable);
                    if (member.Exponent != 1) {
                        sb.Append("^").Append(member.Exponent);
                    }
                }

                sb.Append(" ");
            }

            sb.Append("= 0");

            return sb.ToString();
        }
    }
}