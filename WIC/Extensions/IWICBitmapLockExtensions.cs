using System.ComponentModel;

namespace WIC
{
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static class IWICBitmapLockExtensions
    {
        public static WICSize GetSize(this IWICBitmapLock bitmapLock)
        {
            int width, height;
            bitmapLock.GetSize(out width, out height);
            return new WICSize(width, height);
        }
    }
}
