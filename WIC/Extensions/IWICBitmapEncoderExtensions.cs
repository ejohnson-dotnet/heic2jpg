using System.ComponentModel;
using System.IO;

namespace WIC
{
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static class IWICBitmapEncoderExtensions
    {
        public static IWICMetadataBlockWriter AsMetadataBlockWriter(this IWICBitmapEncoder bitmapEncoder)
        {
            return (IWICMetadataBlockWriter)bitmapEncoder;
        }

        public static IWICBitmapFrameEncode CreateNewFrame(this IWICBitmapEncoder bitmapEncoder, out IPropertyBag2 encoderOptions)
        {
            IPropertyBag2? encoderOptionsNullable = null;
            bitmapEncoder.CreateNewFrame(out IWICBitmapFrameEncode frameEncode, ref encoderOptionsNullable);
            encoderOptions = encoderOptionsNullable!;
            return frameEncode;
        }

        public static IWICBitmapFrameEncode CreateNewFrame(this IWICBitmapEncoder bitmapEncoder)
        {
            bitmapEncoder.CreateNewFrame(out IWICBitmapFrameEncode ppIFrameEncode, null);
            return ppIFrameEncode;
        }

        public static void Initialize(this IWICBitmapEncoder bitmapEncoder, Stream stream, WICBitmapEncoderCacheOption cacheOption)
        {
            bitmapEncoder.Initialize(stream.AsCOMStream(), cacheOption);
        }
    }
}
