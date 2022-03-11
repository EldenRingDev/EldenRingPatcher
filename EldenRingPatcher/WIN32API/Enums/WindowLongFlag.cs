namespace EldenRingPatcher.WIN32API.Enums
{
    enum WindowLongFlag : int
    {
        // [in] nIndex params
        GWL_EXSTYLE = -20,
        GWL_HINSTANCE = -6,
        GWL_HWNDPARENT = -8,
        GWL_ID = -12,
        GWL_STYLE = -16,
        GWL_USERDATA = -21,
        GWL_WNDPROC = -4,
        // [in] dwNewLong params
        DWL_MSGRESULT = 0,
        DWL_DLGPROC = 4,
        DWL_USER = 8
    }
}