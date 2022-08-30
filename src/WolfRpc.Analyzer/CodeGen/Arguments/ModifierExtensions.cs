using System;
using System.Collections.Generic;
using System.Text;

namespace WolfRpc.Analyzer.CodeGen.Arguments;

public static class ModifierExtensions
{
    public static string ToSource(this Modifier modifier)
    {
        var tags = new List<string>();
        if (modifier.HasFlag(Modifier.Ref))
        {
            tags.Add("ref");
        }
        if (modifier.HasFlag(Modifier.Out))
        {
            tags.Add("out");
        }

        return String.Join(" ", tags);
    }
}
