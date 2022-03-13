using System.Runtime.InteropServices;

namespace EldenRingPatcher.WIN32API.Structures
{
    // POINT structure (windef.h)
    [StructLayout(LayoutKind.Sequential)]
    public class Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() =>
            $"X : {X:d}, Y : {Y:d}";
    }
}