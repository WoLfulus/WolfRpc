using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.Text;


namespace WolfRpc.Analyzer.Test.Utilities
{
    public static class SourceTextExtensions
    {
        public static string Source(this SourceText text)
        {
            var value = (string)text.GetType().GetProperty("Source").GetValue(text);
            return value;
        }
    }
}
