using System;

namespace WIC
{
    public struct WICSize : IEquatable<WICSize>
    {
        public WICSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }
        public int Height { get; }

        public override bool Equals(object? obj)
        {
            return obj is WICSize size && Equals(size);
        }

        public bool Equals(WICSize other)
        {
            return Width == other.Width
                && Height == other.Height;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Width, Height);
        }

        public static bool operator ==(WICSize obj1, WICSize obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(WICSize obj1, WICSize obj2)
        {
            return !obj1.Equals(obj2);
        }
    }
}
