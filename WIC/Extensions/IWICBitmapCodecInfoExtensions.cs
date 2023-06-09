using System;
using System.ComponentModel;

namespace WIC
{
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static class IWICBitmapCodecInfoExtensions
    {
        public static string GetColorManagementVersion(this IWICBitmapCodecInfo bitmapCodecInfo)
        {
            return FetchIntoBufferHelper.FetchString(bitmapCodecInfo.GetColorManagementVersion);
        }

        public static string GetDeviceManufacturer(this IWICBitmapCodecInfo bitmapCodecInfo)
        {
            return FetchIntoBufferHelper.FetchString(bitmapCodecInfo.GetDeviceManufacturer);
        }

        public static string GetDeviceModels(this IWICBitmapCodecInfo bitmapCodecInfo)
        {
            return FetchIntoBufferHelper.FetchString(bitmapCodecInfo.GetDeviceModels);
        }

        public static string[] GetMimeTypes(this IWICBitmapCodecInfo bitmapCodecInfo)
        {
            return FetchIntoBufferHelper.FetchString(bitmapCodecInfo.GetMimeTypes).Split(',');
        }

        public static string[] GetFileExtensions(this IWICBitmapCodecInfo bitmapCodecInfo)
        {
            return FetchIntoBufferHelper.FetchString(bitmapCodecInfo.GetFileExtensions).Split(',');
        }
    }
}
