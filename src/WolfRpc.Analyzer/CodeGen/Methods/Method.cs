using System;
using System.Collections.Generic;
using System.Text;

using WolfRpc.Analyzer.CodeGen.Source;
using WolfRpc.Analyzer.Extensions;
using WolfRpc.CodeGen;
using WolfRpc.CodeGen.Arguments;

namespace WolfRpc.Analyzer.CodeGen.Methods;

public class Method : SourceBlock
{
    public string Name { get; set; } = "";
    public string ReturnType { get; set; } = "int";
    public ArgumentList Arguments { get; set; } = new();
    public Visibility Visibility { get; set; } = Visibility.Public;
    public Modifier Modifiers { get; set; } = Modifier.None;
    public string Body { get; set; } = "";

    public ulong Hash
    {
        get
        {
            return $"{ReturnType} {Name}({SourceString.JoinWith(", ", Arguments.Arguments.Select(arg => arg.Type).ToArray())})".Hash();
        }
    }

    public Method(string returnType, string name, IEnumerable<Argument>? arguments = null, Visibility visibility = Visibility.Public, Modifier modifiers = Modifier.None, string body = "")
    {
        Name = name;
        ReturnType = returnType;
        Visibility = visibility;
        Modifiers = modifiers;
        Body = body;
        if (arguments != null)
        {
            Arguments.Add(arguments);
        }
    }

    public override string ToSource()
    {
        var declaration = $"{SourceString.JoinWith(" ", new object[] { Visibility.ToSource(), Modifiers.ToSource(), ReturnType, Name })}({Arguments})";
        if (Modifiers.HasFlag(Modifier.Abstract))
        {
            return $"{declaration};";
        }
        else if (Modifiers.HasFlag(Modifier.Virtual) && Body == "")
        {
            return $"{declaration}\n{{\n\tthrow new NotImplementedException();\n}}";
        }
        return $"{declaration}\n{{\n{SourceString.Indent(Body, 1)}\n}}";
    }

    public string ToSwitchCall(string variable)
    {
        return $"{Hash} => this.{Name}({Arguments.ToUsage(variable)})";
    }
}
