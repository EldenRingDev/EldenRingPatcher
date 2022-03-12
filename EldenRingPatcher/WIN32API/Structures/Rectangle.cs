namespace EldenRingPatcher.WIN32API.Structures
{
    // RECT structure (windef.h)
    public struct Rectangle
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public override string ToString() =>
            $"Left : {Left:d}, Top : {Top:d}, Right : {Right:d}, Bottom : {Bottom:d}";
    }
}