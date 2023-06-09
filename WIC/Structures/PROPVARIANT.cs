using System;
using System.Runtime.InteropServices;

namespace WIC
{
    [StructLayout(LayoutKind.Explicit, Pack = 8)]
    public struct PROPVARIANT
    {
        [FieldOffset(0)]
        public VARTYPE Type;

        [FieldOffset(8)]
        public sbyte I1;

        [FieldOffset(8)]
        public byte UI1;

        [FieldOffset(8)]
        public short I2;

        [FieldOffset(8)]
        public ushort UI2;

        [FieldOffset(8)]
        public int I4;

        [FieldOffset(8)]
        public uint UI4;

        [FieldOffset(8)]
        public long I8;

        [FieldOffset(8)]
        public ulong UI8;

        [FieldOffset(8)]
        public float R4;

        [FieldOffset(8)]
        public double R8;

        [FieldOffset(8)]
        public IntPtr Ptr;

        [FieldOffset(8)]
        public PROPVARIANT_Vector Vector;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PROPVARIANT_Vector
    {
        public int Length;
        public IntPtr Ptr;
    }
}
