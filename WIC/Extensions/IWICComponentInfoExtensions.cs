using System.ComponentModel;

namespace WIC
{
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static class IWICComponentInfoExtensions
    {
        public static string GetAuthor(this IWICComponentInfo componentInfo)
        {
            return FetchIntoBufferHelper.FetchString(componentInfo.GetAuthor);
        }

        public static string GetFriendlyName(this IWICComponentInfo componentInfo)
        {
            return FetchIntoBufferHelper.FetchString(componentInfo.GetFriendlyName);
        }

        public static string GetVersion(this IWICComponentInfo componentInfo)
        {
            return FetchIntoBufferHelper.FetchString(componentInfo.GetVersion);
        }

        public static string GetSpecVersion(this IWICComponentInfo componentInfo)
        {
            return FetchIntoBufferHelper.FetchString(componentInfo.GetSpecVersion);
        }
    }
}
