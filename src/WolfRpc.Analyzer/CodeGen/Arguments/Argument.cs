using System;
using System.Collections.Generic;
using System.Text;

using WolfRpc.Analyzer.CodeGen.Arguments;
using WolfRpc.Analyzer.CodeGen.Source;

namespace WolfRpc.CodeGen.Arguments;

public class Argument : SourceBlock
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";

    public Modifier Modifiers { get; set; } = Modifier.None;

    public override string ToSource()
    {
        return SourceString.JoinWith(" ", new object[] { Modifiers.ToSource(), Type, Name });
    }
}
