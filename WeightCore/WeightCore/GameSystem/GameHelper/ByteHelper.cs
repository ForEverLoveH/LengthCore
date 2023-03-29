﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightCore.GameSystem.GameHelper
{
    public class ByteHelper
    {
        public static byte[] GetBytes(string parseString)
        {
            return Encoding.UTF8.GetBytes(parseString);
        }
        public static string ParseToString(byte[] utf8Bytes)
        {
            return Encoding.UTF8.GetString(utf8Bytes);
        }
    }
}
