using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WIC
{
    public static class IPropertyBag2Extension
    {
        public static object? Read(this IPropertyBag2 propertyBag2, string propertyName)
        {
            PROPBAG2 propBag = new PROPBAG2();
            propBag.pstrName = Marshal.StringToCoTaskMemUni(propertyName);
            try
            {
                object?[] values = new object?[1];
                int[] errors = new int[1];
                propertyBag2.Read(1, new[] { propBag }, null, values, errors);
                if (errors[0] !=0 ) 
                {
                    throw new COMException("Could not read property.", errors[0]);
                }
                return values[0];
            }
            finally
            {
                Marshal.FreeCoTaskMem(propBag.pstrName);
            }
        }

        public static void Write(this IPropertyBag2 propertyBag2, string propertyName, object? value)
        {
            PROPBAG2 propBag = new PROPBAG2();
            propBag.pstrName = Marshal.StringToCoTaskMemUni(propertyName);
            try
            {
                propertyBag2.Write(1, new[] { propBag }, new[] { value });
            }
            finally
            {
                Marshal.FreeCoTaskMem(propBag.pstrName);
            }
        }

    }
}
