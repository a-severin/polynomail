using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polynomial {
    class Program {
        static void Main(string[] args) {
            if (args.Length == 1) {
                var inputFilePath = args[0];
                if (!File.Exists(inputFilePath)) {
                    Console.WriteLine("Provided Input File does not exist.");
                    Console.ReadKey();
                    return;
                }
                var outputFilePath = inputFilePath + ".out";
                var sources = File.ReadAllLines(inputFilePath);
                var results = new string[sources.Length];
                for (int i = 0; i < sources.Length; i++) {
                    try {
                        results[i] = new PolynomialExpressionParser().Parse(sources[i]).CollectAllTerms().Serialize();
                    } catch (Exception exc) {
                        results[i] = exc.Message;
                    }
                }
                File.WriteAllLines(outputFilePath, results);
                return;
            }
            _loop();
        }

        private static void _loop() {
            while (true) {
                Console.WriteLine("Input Polynomial Expression:");
                var input = Console.ReadLine();

                var expression = new PolynomialExpressionParser().Parse(input);
                Console.WriteLine(expression.CollectAllTerms().Serialize());
                Console.WriteLine();
            }
        }
    }
}
