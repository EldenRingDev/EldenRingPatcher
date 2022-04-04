using System;

namespace EldenRingPatcher.Memory
{
    public class MemPOI
    {
        public string Name { get; set; }
        public string Pattern { get; set; }
        public string Comment { get; set; }
        public IntPtr Address { get; set; } = IntPtr.Zero;
        public int[] Offsets { get; set; }

        MemPOI PlayerCoords = new()
        {
            Name = "Pointer to player coordinates",
            Pattern = "f3 0f 10 87 ?? ?? ?? ?? 4c 8d 4d",
        };
    }
}