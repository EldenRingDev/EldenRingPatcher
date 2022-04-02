using System;

namespace EldenRingPatcher.Memory
{
    public class Offsets
    {
        public static IntPtr PointerToPlayerCoords =>
            MemScan.FindOffsetFromAOB("f3 0f 10 87 ?? ?? ?? ?? 4c 8d 4d");
    }
}