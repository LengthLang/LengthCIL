using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace LengthCIL.CLI {
    class Options {
        [Value(0, MetaName = "input file",
            HelpText = "Input file to be processed.",
            Required = true)]
        public string FileName { get; set; }
        [Option('o', "output", HelpText = "The file to output to", Required = true)]
        public string OutputFile { get; set; }
    }
}
