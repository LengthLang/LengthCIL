using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace LengthCIL.Compiler {
    public class LengthCompiler {
        public List<int> Program { get; set; }

        public void Parse(string text) {
            foreach (string line in text.Split('\n')) {
                Program.Add(line.Length);
            }
        }

        public void Compile(string name) {
            var codeName = name.Split('.')[0];
            var assembly = AssemblyDefinition.CreateAssembly(
                new AssemblyNameDefinition(codeName, new Version()),
                codeName,
                ModuleKind.Console);

            var module = assembly.MainModule;
            var programType = new TypeDefinition(
                codeName, 
                "Length", 
                TypeAttributes.Public, 
                module.ImportReference(typeof(object))
            );

            module.Types.Add(programType);
            var mainMethod = new MethodDefinition(
                "Main",
                MethodAttributes.Public | MethodAttributes.Static,
                module.ImportReference(typeof(void))
            );

            programType.Methods.Add(mainMethod);

            var il = mainMethod.Body.GetILProcessor();
            il.Emit(OpCodes.Ldstr, "Hello, World!");

            var writeLine = module.ImportReference(
                typeof(Console).GetMethod(
                    "WriteLine",
                    new Type[] { typeof(string) }
                )
            );
            il.Emit(OpCodes.Call, writeLine);
            il.Emit(OpCodes.Ret);

            assembly.EntryPoint = mainMethod;
            assembly.Write(name);
        }
    }
}
