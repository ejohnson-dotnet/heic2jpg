using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace WIC
{
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static class IWICMetadataQueryReaderExtensions
    {
        /// <summary>
        /// Retrieves the current path relative to the root metadata block.
        /// </summary>       
        /// <returns>The current namespace location.</returns>
        /// <remarks>
        /// If the query reader is relative to the top of the metadata hierarchy, it will return a single-char string.
        /// <br/>
        /// If the query reader is relative to a nested metadata block, this method will return the path to the current query reader.
        /// </remarks>
        public static string GetLocation(this IWICMetadataQueryReader metadataQueryReader)
        {
            return FetchIntoBufferHelper.FetchString(metadataQueryReader.GetLocation);
        }

        /// <summary>
        /// Retrieves the metadata block or item identified by a metadata query expression.
        /// </summary>
        /// <param name="name">The query expression to the requested metadata block or item.</param>
        /// <returns>The metadata block or item requested.</returns>
        /// <exception cref="COMException">Thrown when the metadata block or item was not found (HRESULT 0x88982F40).</exception>
        /// <remarks>
        /// GetMetadataByName uses metadata query expressions to access embedded metadata. For more information on the metadata query language, see the Metadata Query Language Overview.
        /// <br/>
        /// If multiple blocks or items exist that are expressed by the same query expression, the first metadata block or item found will be returned.
        /// </remarks>
        public static object GetMetadataByName(this IWICMetadataQueryReader metadataQueryReader, string name)
        {
            if (metadataQueryReader is null)
            {
                throw new NullReferenceException();
            }
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var variant = new PROPVARIANT();
            try
            {
                metadataQueryReader.GetMetadataByName(name, ref variant);
                return PropVariantHelper.Decode(variant);
            }
            finally
            {
                PropVariantHelper.Free(variant);
            }
        }

        /// <summary>
        /// Retrieves the metadata block or item identified by a metadata query expression.
        /// </summary>
        /// <param name="name">The query expression to the requested metadata block or item.</param>
        /// <param name="value">The metadata block or item requested or null if not found.</param>
        /// <returns>True when the metadata block or item was found, otherwise false.</returns>
        /// <remarks>
        /// GetMetadataByName uses metadata query expressions to access embedded metadata. For more information on the metadata query language, see the Metadata Query Language Overview.
        /// <br/>
        /// If multiple blocks or items exist that are expressed by the same query expression, the first metadata block or item found will be returned.
        /// </remarks>
        public static bool TryGetMetadataByName(this IWICMetadataQueryReader metadataQueryReader, string name, out object? value)
        {
            if (metadataQueryReader is null)
            {
                throw new NullReferenceException();
            }
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var variant = new PROPVARIANT();
            try
            {
                metadataQueryReader.GetMetadataByName(name, ref variant);
                value = PropVariantHelper.Decode(variant);
                return true;
            }
            catch (COMException ex) when (ex.ErrorCode == WinCodecError.PROPERTY_NOT_FOUND)
            {
                value = null;
                return false;
            }
            finally
            {
                PropVariantHelper.Free(variant);
            }
        }

        /// <summary>
        /// Gets the names of all metadata items at the current relative location within the metadata hierarchy.
        /// </summary>
        /// <returns>An enumerable that contains query strings that can be used in the current <see cref="IWICMetadataQueryReader"/>.</returns>
        /// <remarks>
        /// The retrieved enumerable only contains query strings for the metadata blocks and items in the current level of the hierarchy.
        /// </remarks>
        public static IEnumerable<string> GetNames(this IWICMetadataQueryReader metadataQueryReader)
        {
            return metadataQueryReader.GetEnumerator().AsEnumerable();
        }

        public static IEnumerable<string> GetNamesRecursive(this IWICMetadataQueryReader reader)
        {
            foreach (var name in reader.GetNames())
            {
                var val = reader.GetMetadataByName(name);
                if (val is IWICMetadataQueryReader)
                {
                    foreach (var childName in GetNamesRecursive((IWICMetadataQueryReader)val))
                        yield return name + childName;
                }
                else
                    yield return name;
            }
        }
    }
}
