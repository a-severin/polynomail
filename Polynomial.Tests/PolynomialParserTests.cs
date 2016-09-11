using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polynomial.Tests {
    [TestFixture]
    public class PolynomialParserTests {
        [Test]
        public void Parse() {
            //var source = "-2x^2 + 3.5xy) + (4y - 6x)/2 = (y^2 - xy)*2 + y";
            //var source = "-2x^2 + 3.5xy + 4y - 6x + 5 = y^2 - xy + y - 7";
            //var source = "-2x^2 + 3.5xy + 4y - 6x + 5 = xy";
            //var source = "(-2x^2) + (3.5xy) + (4y - 6x + 5) = (y^2 - xy) + (y - 7)";
            var source = "(-2x^2 + (3.5xy + (4y) - 6x) + 5) = (y^2 - xy) + (y - 7)";

            new PolynomialParser().Parse(source);
        }
    }
}
