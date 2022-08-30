using System;
using System.Collections.Generic;
using System.Text;

using WolfRpc.Analyzer.CodeGen.Arguments;
using WolfRpc.Analyzer.CodeGen.Source;

namespace WolfRpc.CodeGen.Arguments;

public class ArgumentList : SourceBlock
{
    public List<Argument> Arguments { get; set; } = new();

    public void Add(Argument argument)
    {
        this.Arguments.Add(argument);
    }

    public void Add(IEnumerable<Argument> arguments)
    {
        foreach (var argument in arguments)
            this.Arguments.Add(argument);
    }

    public void Clear()
    {
        this.Arguments.Clear();
    }

    public override string ToSource()
    {
        return SourceString.JoinWith(", ", Arguments.ToArray());
    }

    public string ToUsage(string variable)
    {
        return SourceString.JoinWith(", ", Arguments.Select((arg, index) => $"(({arg.Type})({variable}[{index}]))").ToArray());
    }
}
