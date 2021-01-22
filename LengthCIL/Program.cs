using CommandLine;
using System;
using System.IO;
using LengthCIL.Compiler;

namespace LengthCIL.CLI {

    class Program {
        static void Main(string[] args) {
            Parser.Default.ParseArguments<Options>(args).WithParsed(o => {
                var text = File.ReadAllText(o.FileName);
                var compiler = new LengthCompiler();
                compiler.Parse(text);
                compiler.Compile(o.OutputFile);
            });
        }
    }
}
