﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace example
{
    internal static class ErrorCheckingExten
    {
        public static int AssertZero(this int value, Func<string> error)
        {
            if (value != 0)
                throw new Exception(error());
            
            return value;
        }

        public static nint AssertNotNull(this nint value, Func<string> error)
        {
            if (value == IntPtr.Zero)
                throw new Exception(error());
            
            return value;
        }
    }
}
