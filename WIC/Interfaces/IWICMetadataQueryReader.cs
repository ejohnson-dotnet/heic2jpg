using System;
using System.Runtime.InteropServices;

namespace WIC
{
    /// <summary>
    /// Exposes methods for retrieving metadata blocks and items from a decoder or its image frames using a metadata query expression.
    /// </summary>
    [ComImport]
    [Guid(IID.IWICMetadataQueryReader)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWICMetadataQueryReader
    {
        /// <summary>
        /// Gets the metadata query readers container format.
        /// </summary>       
        /// <returns>The metadata query readers cointainer format GUID.</returns>
        Guid GetContainerFormat();

        /// <summary>
        /// Retrieves the current path relative to the root metadata block.
        /// </summary>       
        /// <param name="cchMaxLength">The length of the wzNamespace buffer.</param>
        /// <param name="wzNamespace">Pointer that receives the current namespace location.</param>
        /// <param name="pcchActualLength">The actual buffer length that was needed to retrieve the current namespace location.</param>
        /// <remarks>
        /// If you pass NULL to wzNamespace, GetLocation ignores cchMaxLength and returns the required buffer length to store the path in the variable that pcchActualLength points to.
        /// <br/>
        /// If the query reader is relative to the top of the metadata hierarchy, it will return a single-char string.
        /// <br/>
        /// If the query reader is relative to a nested metadata block, this method will return the path to the current query reader.
        /// </remarks>
        void GetLocation(
            [In] int cchMaxLength,
            [In, Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeParamIndex = 0)] char[]? wzNamespace,
            [Out] out int pcchActualLength);

        /// <summary>
        /// Retrieves the metadata block or item identified by a metadata query expression.
        /// </summary>      
        /// <param name="wzName">The query expression to the requested metadata block or item.</param>
        /// <param name="pvarValue">When this method returns, contains the metadata block or item requested.</param>
        ///  <remarks>
        /// GetMetadataByName uses metadata query expressions to access embedded metadata. For more information on the metadata query language, see the Metadata Query Language Overview.
        /// <br/>
        /// If multiple blocks or items exist that are expressed by the same query expression, the first metadata block or item found will be returned.
        /// </remarks>
        void GetMetadataByName(
            [In, MarshalAs(UnmanagedType.LPWStr)] string wzName,
            [In, Out, MarshalAs(UnmanagedType.Struct)] ref PROPVARIANT pvarValue);

        /// <summary>
        /// Gets an enumerator of all metadata items at the current relative location within the metadata hierarchy.
        /// </summary>
        /// <returns>An enumerator that contains query strings that can be used in the current <see cref="IWICMetadataQueryReader"/>.</returns>
        /// <remarks>
        /// The retrieved enumerator only contains query strings for the metadata blocks and items in the current level of the hierarchy.
        /// </remarks>
        IEnumString GetEnumerator();
    }
}
