namespace WIC
{
    /// <summary>
    /// <see cref="https://docs.microsoft.com/de-de/windows/win32/wic/-wic-codec-error-codes"/>
    /// </summary>
    public static class WinCodecError
    {
        public const int PROPERTY_NOT_FOUND = unchecked((int)0x88982F40);
        public const int PROPERTY_NOT_SUPPORTED = unchecked((int)0x88982F41);
        public const int UNSUPPORTED_OPERATION = unchecked((int)0x88982F81);
        public const int TOO_MUCH_METADATA = unchecked((int)0x88982F52);
        public const int INSUFFICIENT_BUFFER = unchecked((int)0x88982F8C);
        public const int IMAGE_METADATA_HEADER_UNKNOWN = unchecked((int)0x88982F63);
        public const int COMPONENT_NOT_FOUND = unchecked((int)0x88982F50);
        public const int CODEC_NO_THUMBNAIL = unchecked((int)0x88982F44);
        public const int PALETTE_UNAVAILABLE = unchecked((int)0x88982F45);
    }
}
