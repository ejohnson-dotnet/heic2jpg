using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace WIC
{
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static class IWICMetadataQueryWriterExtensions
    {
        /// <summary>
        /// Sets a metadata item to a specific location.
        /// </summary>
        /// <param name="name">The name of the metadata item.</param>
        /// <param name="value">The metadata to set.</param>
        ///  <remarks>
        /// SetMetadataByName uses metadata query expressions to set metadata. For more information on the metadata query language, see the Metadata Query Language Overview.
        /// </remarks>
        public static void SetMetadataByName(this IWICMetadataQueryWriter metadataQueryWriter, string name, object value)
        {
            if (metadataQueryWriter is null)
            {
                throw new NullReferenceException();
            }
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var variant = PropVariantHelper.Encode(value);
            try
            {
                metadataQueryWriter.SetMetadataByName(name, ref variant);
            }
            finally
            {
                PropVariantHelper.Free(variant);
            }
        }

    }
}
