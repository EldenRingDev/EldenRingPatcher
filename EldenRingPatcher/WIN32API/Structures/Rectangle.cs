using System.Runtime.InteropServices;

namespace EldenRingPatcher.WIN32API.Structures
{
    // RECT structure (windef.h)
    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public Rectangle(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public override string ToString() =>
            $"Left : {Left:d}, Top : {Top:d}, Right : {Right:d}, Bottom : {Bottom:d}";
    }
}