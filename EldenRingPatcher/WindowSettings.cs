namespace EldenRingPatcher
{
    public static class WindowSettings
    {
        public static int WindowTitleMaxLength { get; set; } = 100;    // Maximum length of window title before its truncated
        public static int ValidateHandleThreshold { get; set; } = 10;  // How often the user selected window handle gets validated
        public static int ClippingRefreshInterval { get; set; } = 100; // How often the clipped area is refreshed in milliseconds
        public static bool VerboseOutput { get; set; }
    }
}