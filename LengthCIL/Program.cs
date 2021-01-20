using CommandLine;
using System;
using System.IO;
using LengthCIL.Compiler;

namespace LengthCIL.CLI {

    class Program {
        static void Main(string[] args) {
            Parser.Default.ParseArguments<Options>(args).WithParsed(o => {
                new LengthCompiler().Compile(o.OutputFile);
                Console.WriteLine(o.OutputFile);
            });
        }
    }
}
