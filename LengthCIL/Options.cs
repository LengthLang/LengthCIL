using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace LengthCIL.CLI {
    class Options {
        [Option('o', "output", HelpText = "The file to output to", Required = true)]
        public string OutputFile { get; set; }
    }
}
