using System;
using System.Collections.Generic;
using System.Text;

namespace WIC
{
    public class WICBlob
    {
        public byte[] Bytes { get; }

        public WICBlob(byte[] bytes)
        {
            Bytes = bytes;
        }
    }
}
