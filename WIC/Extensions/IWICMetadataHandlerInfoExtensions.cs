using System;
using System.ComponentModel;

namespace WIC
{
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static class IWICMetadataHandlerInfoExtensions
    {
        public static Guid[] GetContainerFormats(this IWICMetadataHandlerInfo metadataHandlerInfo)
        {
            return FetchIntoBufferHelper.FetchArray<Guid>(metadataHandlerInfo.GetContainerFormats);
        }

        public static string GetDeviceManufacturer(this IWICMetadataHandlerInfo metadataHandlerInfo)
        {
            return FetchIntoBufferHelper.FetchString(metadataHandlerInfo.GetDeviceManufacturer);
        }

        public static string GetDeviceModels(this IWICMetadataHandlerInfo metadataHandlerInfo)
        {
            return FetchIntoBufferHelper.FetchString(metadataHandlerInfo.GetDeviceModels);
        }
    }
}
