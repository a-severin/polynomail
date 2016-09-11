using System;
using NUnit.Framework;

namespace Polynomial.Tests {
    [TestFixture]
    public class PolynomialExpressionTests {
        [Test]
        public void CollectAllTerms_CollectSimilarTerms() {
            var expresstion = new PolynomialExpression();
            expresstion.AddMember(new PolynomialMember("x", 2D, 1));
            expresstion.AddMember(new PolynomialMember("x", 2D, 1));

            expresstion.AddMember(new PolynomialMember("x", 2D, 2));
            expresstion.AddMember(new PolynomialMember("x", 2D, 2));

            expresstion.AddMember(new PolynomialMember("y", 2D, 2));
            expresstion.AddMember(new PolynomialMember("y", -2D, 2));

            var collectedExpression = expresstion.CollectAllTerms();
            Console.WriteLine($"{expresstion.Serialize()} => {collectedExpression.Serialize()}");

            var result = collectedExpression.Serialize();

            Assert.IsTrue(result.Contains("4x"));
            Assert.IsTrue(result.Contains("4x^2"));
            Assert.IsFalse(result.Contains("y"));
        }

        [Test]
        public void Serialize() {
            var expression = new PolynomialExpression();
            expression.AddMember(new PolynomialMember("x", 3D, 2));
            expression.AddMember(new PolynomialMember("xy", -4.1, 3));
            expression.AddMember(new PolynomialMember("y", 1, 1));
            expression.AddMember(new PolynomialMember("", -5, 0));

            Assert.AreEqual("3x^2 - 4.1xy^3 + y - 5 = 0", expression.Serialize());
        }
    }
}