using System;
using System.Collections.Generic;
using System.Text;

using WolfRpc.Analyzer.Hashing;

namespace WolfRpc.Analyzer.Extensions;

public static class StringExtensions
{
    public static ulong Hash(this string value)
    {
        return BitConverter.ToUInt64(XxHash64.Hash(Encoding.Default.GetBytes(value)), 0);
    }
}
