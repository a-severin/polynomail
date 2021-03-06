﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polynomial.Tests {
    [TestFixture]
    public class PolynomialParserTests {
        [Test]
        public void ParseTest() {
            var source = "2(2x^2 + 2*(2xy + (2y)/2 - 2x) + 2) = x";

            var expression = new PolynomialExpressionParser().Parse(source);

            Assert.AreEqual("4x^2 + 8xy + 4y - 8x + 4 - x = 0", expression.Serialize());
        }
    }
}
