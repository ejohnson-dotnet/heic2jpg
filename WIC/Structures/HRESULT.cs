using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIC
{
    public struct HRESULT
    {

        /// <summary>
        /// If the result is not a success, then throw the appropriate exception.
        /// </summary>
        /// <param name="hr"></param>
        /// <ExternalAPI/>
        /// <SecurityNote>
        ///   Critical: This code calls into Marshal.GetExceptionForHR which has a link demand on it
        ///   Safe: Throwing an exception is deemed as a safe operation (throwing exceptions is allowed in Partial Trust). 
        ///         We ensure the call to GetExceptionForHR is safe since we pass an IntPtr that has a value of -1 so that 
        ///         GetExceptionForHR ignores IErrorInfo of the current thread, which could reveal critical information otherwise.
        /// </SecurityNote>
        [System.Security.SecuritySafeCritical]
        internal static Exception ConvertHRToException(int hr)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return System.Runtime.InteropServices.Marshal.GetExceptionForHR(hr);
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        /// If the result is not a success, then throw the appropriate exception.
        /// </summary>
        /// <param name="hr"></param>
        /// <ExternalAPI/>
        public static void Check(int hr)
        {
            if (hr >= 0)
            {
                // The call succeeded, don't bother calling Marshal.ThrowExceptionForHr
                return;
            }
            else
            {
                throw ConvertHRToException(hr);
            }
        }

        /// <summary>
        /// HRESULT succeeded.
        /// </summary>
        /// <ExternalAPI/>
        public static bool Succeeded(int hr)
        {
            if (hr >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// HRESULT succeeded.
        /// </summary>
        /// <ExternalAPI/>
        public static bool Failed(int hr)
        {
            return !(HRESULT.Succeeded(hr));
        }

        public const int S_OK = 0;
        public const int E_FAIL = unchecked((int)0x80004005);
        public const int E_OUTOFMEMORY = unchecked((int)0x8007000E);
        public const int D3DERR_OUTOFVIDEOMEMORY = unchecked((int)0x8876017C);
    }

}
