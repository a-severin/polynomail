using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polynomial.Tests {
    [TestFixture]
    public class PolynomialMemberParserTests {
        [Test]
        public void Parse_TermWithAllParts() {
            var member = new PolynomialMemberParser().Parse(new StringBuilder("-2x^2"));

            Assert.AreEqual("x", member.Variable);
            Assert.AreEqual(-2D, member.Coefficient);
            Assert.AreEqual(2, member.Exponent);
        }

        [Test]
        public void Parse_TermWithExponentEqualOne() {
            var member = new PolynomialMemberParser().Parse(new StringBuilder("-2x"));

            Assert.AreEqual("x", member.Variable);
            Assert.AreEqual(-2D, member.Coefficient);
            Assert.AreEqual(1, member.Exponent);
        }

        [Test]
        public void Parse_TermWithCoefficientEqualOne() {
            var member = new PolynomialMemberParser().Parse(new StringBuilder("-x"));

            Assert.AreEqual("x", member.Variable);
            Assert.AreEqual(-1D, member.Coefficient);
            Assert.AreEqual(1, member.Exponent);
        }

        [Test]
        public void Parse_TermWithExponentEqualZero() {
            var member = new PolynomialMemberParser().Parse(new StringBuilder("-5"));

            Assert.AreEqual("", member.Variable);
            Assert.AreEqual(-5D, member.Coefficient);
            Assert.AreEqual(0, member.Exponent);
        }


    }
}
