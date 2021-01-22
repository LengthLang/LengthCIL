using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace LengthCIL.Compiler {
    public class LengthCompiler {
        public List<int> Program { get; set; }

        public void Parse(string text) {
            Program = new List<int>();
            foreach (string line in text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None)) {
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

            var writeLine = module.ImportReference(
                typeof(Console).GetMethod(
                    "WriteLine",
                    new Type[] { typeof(string) }
                )
            );
            var writeInt = module.ImportReference(
                typeof(Console).GetMethod(
                    "Write",
                    new Type[] { typeof(int) }
                )
            );
            var writeChar = module.ImportReference(
                typeof(Console).GetMethod(
                    "Write",
                    new Type[] { typeof(char) }
                )
            );
            var readKey = module.ImportReference(
                typeof(Console).GetMethod(
                    "Read"
                )
            );
            var instructions = new List<Instruction>();
            foreach (var _ in Program) {
                Console.WriteLine(".");
                instructions.Add(il.Create(OpCodes.Nop));
            }
            instructions.Add(il.Create(OpCodes.Nop));
            Console.WriteLine(instructions.Count);
            for (int i = 0; i < Program.Count; i++) {
                var line = Program[i];
                switch (line) {
                    // inp
                    case 9:
                        var insLdInput = il.Create(OpCodes.Ldstr, "Input:");
                        il.InsertAfter(instructions[i], insLdInput);
                        var insWrite = il.Create(OpCodes.Call, writeLine);
                        il.InsertAfter(insLdInput, insWrite);
                        var insReadKey = il.Create(OpCodes.Call, readKey);
                        il.InsertAfter(insWrite, insReadKey);               
                        break;
                    // add 
                    case 10:
                        il.InsertAfter(instructions[i], il.Create(OpCodes.Add));
                        break;
                    // sub
                    case 11:
                        il.InsertAfter(instructions[i], il.Create(OpCodes.Sub));
                        break;
                    // dup
                    case 12:
                        il.InsertAfter(instructions[i], il.Create(OpCodes.Dup));
                        break;
                    // cond
                    case 13:
                        il.InsertAfter(instructions[i], il.Create(OpCodes.Brfalse, instructions[i + 2]));
                        break;
                    // gotou
                    case 14:
                        il.InsertAfter(instructions[i], il.Create(
                            OpCodes.Br, 
                            instructions[Program[i + 1] - 1]
                        ));
                        i++;
                        break;
                    // outn
                    case 15:
                        il.InsertAfter(instructions[i], il.Create(OpCodes.Call, writeInt));
                        break;
                    // outa
                    case 16:
                        il.InsertAfter(instructions[i], il.Create(OpCodes.Call, writeChar));
                        break;
                    // mul
                    case 20:
                        il.InsertAfter(instructions[i], il.Create(OpCodes.Mul));
                        break;
                    // div
                    case 21:
                        il.InsertAfter(instructions[i], il.Create(OpCodes.Div));
                        break;
                    // push
                    case 25:
                        il.InsertAfter(instructions[i], il.Create(OpCodes.Ldc_I4, Program[i + 1]));
                        i++;
                        break;
                }
            }
            il.Emit(OpCodes.Ret);

            assembly.EntryPoint = mainMethod;
            assembly.Write(name);
        }
    }
}
