using NLog;
using Reloaded.Assembler;
using System;
using System.Collections.Generic;

namespace EldenRingPatcher.Memory
{
    class MemAssembly
    {
        private static readonly Logger AssemblyLog = LogManager.GetLogger("MemAssembly");

        public static byte[] AssembleOpcodes(List<string> instructions)
        {
            List<string> mnemonics = new() { "use64" }; // Elden Ring is 64bit app

            for (int i = 0; i < instructions.Count; i++)
            {
                mnemonics.Add(instructions[i]);
            }

            Assembler assembler = new();

            try
            {
                byte[] opcodes = assembler.Assemble(mnemonics);
#if DEBUG
                AssemblyLog.Info("Resulting opcodes {0:x}", opcodes);
#endif
                assembler.Dispose();
                GC.Collect();
                return opcodes;
            }
            catch(Exception ex)
            {
                AssemblyLog.Log(LogLevel.Error, ex);
                assembler.Dispose();
                GC.Collect();
                return null;
            }
        }
    }
}