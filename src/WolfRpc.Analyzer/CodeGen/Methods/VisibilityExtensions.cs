using System;
using System.Collections.Generic;
using System.Text;

namespace WolfRpc.Analyzer.CodeGen.Methods;

public static class VisibilityExtensions
{
    public static string ToSource(this Visibility visibility)
    {
        return visibility switch
        {
            Visibility.Private => "private",
            Visibility.Protected => "protected",
            Visibility.Public => "public",
            _ => ""
        };
    }
}
