﻿using System.ComponentModel;

namespace WIC
{
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static class IWICColorContextExtensions
    {
        public static void InitializeFromMemory(this IWICColorContext colorContext, byte[] pbBuffer)
        {
            colorContext.InitializeFromMemory(pbBuffer, pbBuffer.Length);
        }

        public static byte[] GetProfileBytes(this IWICColorContext colorContext)
        {
            return FetchIntoBufferHelper.FetchArray<byte>(colorContext.GetProfileBytes);
        }
    }
}
