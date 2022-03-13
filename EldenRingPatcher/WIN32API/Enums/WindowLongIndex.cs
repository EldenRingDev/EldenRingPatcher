﻿namespace EldenRingPatcher.WIN32API.Enums
{
    internal enum WindowLongIndex
    {
        // [in] nIndex params
        GWL_WNDPROC    = -4,
        GWL_HINSTANCE  = -6,
        GWL_HWNDPARENT = -8,
        GWL_ID         = -12,
        GWL_STYLE      = -16,
        GWL_EXSTYLE    = -20,
        GWL_USERDATA   = -21,
        // [in] dwNewLong params
        DWL_MSGRESULT  = 0,
        DWL_DLGPROC    = 4,
        DWL_USER       = 8
    }
}