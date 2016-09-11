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
                //todo
            }
            _loop();
        }

        private static void _loop() {
            while (true) {
                Console.WriteLine("Input Polynomial Expression:");
                var input = Console.ReadLine();

                //todo
            }
        }
    }
}
