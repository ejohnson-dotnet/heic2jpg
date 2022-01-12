using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WIC
{
    [ComImport]
    [Guid(IID.IWICPixelFormatInfo2)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWICPixelFormatInfo2 : IWICPixelFormatInfo
    {
        #region Members inherited from `IWICComponentInfo`

        new WICComponentType GetComponentType();

        new Guid GetCLSID();

        new WICComponentSigning GetSigningStatus();

        new void GetAuthor(
            [In] int cchAuthor,
            [In, Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeParamIndex = 0)] char[]? wzAuthor,
            [Out] out int pcchActual);

        new Guid GetVendorGUID();

        new void GetVersion(
            [In] int cchVersion,
            [In, Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeParamIndex = 0)] char[]? wzVersion,
            [Out] out int pcchActual);

        new void GetSpecVersion(
            [In] int cchSpecVersion,
            [In, Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeParamIndex = 0)] char[]? wzSpecVersion,
            [Out] out int pcchActual);

        new void GetFriendlyName(
            [In] int cchFriendlyName,
            [In, Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeParamIndex = 0)] char[]? wzFriendlyName,
            [Out] out int pcchActual);

        #endregion

        #region Members inherited from `IWICPixelFormatInfo`

        new Guid GetFormatGUID();

        [return: MarshalAs(UnmanagedType.Interface)]
        new IWICColorContext GetColorContext();

        new int GetBitsPerPixel();

        new int GetChannelCount();

        new void GetChannelMask(
            [In] int uiChannelIndex,
            [In] int cbMaskBuffer,
            [In, Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 1)] byte[] pbMaskBuffer,
            [Out] out int pcbActual);

        #endregion

        [return: MarshalAs(UnmanagedType.Bool)]
        bool SupportsTransparency();
        
        WICPixelFormatNumericRepresentation GetNumericRepresentation();

    }
}
