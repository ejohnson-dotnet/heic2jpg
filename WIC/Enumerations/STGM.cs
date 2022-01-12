using System;
using System.ComponentModel;

namespace WIC
{  
    [Flags]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public enum STGM : int
    {
        STGM_READ = 0x00000000,
        STGM_WRITE = 0x00000001,
        STGM_READWRITE = 0x00000002,
        STGM_SHARE_DENY_NONE = 0x00000040,
        STGM_SHARE_DENY_READ = 0x00000030,
        STGM_SHARE_DENY_WRITE = 0x00000020,
        STGM_SHARE_EXCLUSIVE = 0x00000010,
        STGM_PRIORITY = 0x00040000,
        STGM_CREATE = 0x00001000,
        STGM_CONVERT = 0x00020000,
        STGM_FAILIFTHERE = 0x00000000,
        STGM_DIRECT = 0x00000000,
        STGM_TRANSACTED	= 0x00010000,
        STGM_NOSCRATCH = 0x00100000,
        STGM_NOSNAPSHOT	= 0x00200000,
        STGM_SIMPLE = 0x08000000,
        STGM_DIRECT_SWMR = 0x00400000,
        STGM_DELETEONRELEASE = 0x04000000,
    }
}
