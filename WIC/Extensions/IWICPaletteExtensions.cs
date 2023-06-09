using System.ComponentModel;

namespace WIC
{
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static class IWICPaletteExtensions
    {
        public static int[] GetColors(this IWICPalette palette)
        {
            return FetchIntoBufferHelper.FetchArray<int>(palette.GetColors);
        }

        public static void InitializeCustom(this IWICPalette palette, int[] pColors)
        {
            palette.InitializeCustom(pColors, pColors.Length);
        }
    }
}
