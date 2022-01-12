using System;
using System.ComponentModel;

namespace WIC
{
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static class IWICBitmapSourceExtensions
    {
        public static void CopyPixels(this IWICBitmapSource bitmapSource, int cbStride, byte[] pbBuffer, WICRect? prc = null)
        {
            using (var prcPtr = CoTaskMemPtr.From(prc))
            {
                bitmapSource.CopyPixels(prcPtr, cbStride, pbBuffer.Length, pbBuffer);
            }
        }

        public static IWICPalette? GetColorPalette(this IWICBitmapSource bitmapSource)
        {
            try
            {
                var wic = new WICImagingFactory();
                IWICPalette colorPalette = wic.CreatePalette();
                bitmapSource.CopyPalette(colorPalette);
                return colorPalette;
            }
            catch (Exception exception) when (exception.HResult == WinCodecError.PALETTE_UNAVAILABLE)
            {
                // no color palette available
                return null;
            }
        }

        public static byte[] GetPixels(this IWICBitmapSource bitmapSource)
        {
            var pixelFormatInfo = bitmapSource.GetPixelFormatInfo();
            int bitsPerPixel = pixelFormatInfo.GetBitsPerPixel();
            bitmapSource.GetSize(out int width, out int height);
            int stride = width * bitsPerPixel + 7 / 8;
            byte[] buffer = new byte[height * stride];
            bitmapSource.CopyPixels(stride, buffer);
            return buffer;
        }

        public static WICSize GetSize(this IWICBitmapSource bitmapSource)
        {
            bitmapSource.GetSize(out int width, out int height);
            return new WICSize(width, height);
        }

        public static Resolution GetResolution(this IWICBitmapSource bitmapSource)
        {
            bitmapSource.GetResolution(out double dpiX, out double dpiY);
            return new Resolution(dpiX, dpiY);
        }

        public static IWICPixelFormatInfo GetPixelFormatInfo(this IWICBitmapSource bitmapSource)
        {
            var wic = new WICImagingFactory();
            Guid pixelFormat = bitmapSource.GetPixelFormat();
            var pixelFormatInfo = (IWICPixelFormatInfo)wic.CreateComponentInfo(pixelFormat);
            return pixelFormatInfo;
        }
    }
}
