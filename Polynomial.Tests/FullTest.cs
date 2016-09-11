using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polynomial.Tests {
    [TestFixture]
    public class FullTest {
        [Test]
        public void TestCase() {

            var source = "2(2x^2 + 2*(2xy + (2y^2 - 2x)/2 - y^2) + 2) = -3*(x^2 + (8xy - 4y^2)/2 - 2)";
            
            var expression = new PolynomialExpressionParser().Parse(source);

            Assert.AreEqual("7x^2 - 4x + 20xy - 6y^2 - 2 = 0", expression.CollectAllTerms().Serialize());
        }
    }
}
