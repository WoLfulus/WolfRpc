using System;
using System.Collections.Generic;
using System.Text;

namespace WolfRpc.Analyzer.CodeGen.Methods;

public static class ModifierExtensions
{
    public static string ToSource(this Modifier modifier)
    {
        var tags = new List<string>();
        if (modifier.HasFlag(Modifier.Static))
        {
            tags.Add("static");
        }
        if (modifier.HasFlag(Modifier.Abstract))
        {
            tags.Add("abstract");
        }
        if (modifier.HasFlag(Modifier.Virtual))
        {
            tags.Add("virtual");
        }
        if (modifier.HasFlag(Modifier.Override))
        {
            tags.Add("override");
        }
        return String.Join(" ", tags);
    }
}
