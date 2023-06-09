namespace WIC
{
    internal delegate void FetchIntoBuffer<T>(int size, T[]? buffer, out int length);

    internal static class FetchIntoBufferHelper
    {
        internal static T[] FetchArray<T>(FetchIntoBuffer<T> fetcher)
        {
            fetcher.Invoke(0, null, out int length);
            var buffer = new T[length];
            if (length > 0)
            {
                fetcher.Invoke(length, buffer, out _);
            }
            return buffer;
        }

        internal static string FetchString(FetchIntoBuffer<char> fetcher)
        {
            var buffer = FetchArray(fetcher);
            int length = buffer.Length - 1;
            if (length > 0)
            {
                return new string(buffer, 0, length);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
