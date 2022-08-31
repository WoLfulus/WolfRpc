using System;
using System.Collections.Generic;
using System.Text;

using WolfRpc.Analyzer.CodeGen.Source;
using WolfRpc.CodeGen;
using WolfRpc.CodeGen.Arguments;

namespace WolfRpc.Analyzer.CodeGen.Methods;

public class MethodList : SourceBlock
{
    public List<Method> Methods { get; set; } = new();

    public void Add(Method argument)
    {
        this.Methods.Add(argument);
    }

    public void Add(IEnumerable<Method> arguments)
    {
        foreach (var argument in arguments)
        { 
            this.Methods.Add(argument);
        }
    }

    public void Clear()
    {
        this.Methods.Clear();
    }

    public override string ToSource()
    {
        return SourceString.JoinWith("\r\n\r\n", Methods.Select(method => method.ToSource()).ToArray());
    }

    public string ToSwitchCall(string variable, int indent)
    {
        return SourceString.Indent(SourceString.JoinWith(",\r\n", Methods.Select(method => method.ToSwitchCall(variable)).ToArray()), indent);
    }

    public string ToValidateArgumentCount(string variable, int indent)
    {
        return SourceString.Indent(SourceString.JoinWith(",\r\n", Methods.Select(method => $"{method.Hash} => {variable} == {method.Arguments.Arguments.Count}").ToArray()), indent);
    }
}
