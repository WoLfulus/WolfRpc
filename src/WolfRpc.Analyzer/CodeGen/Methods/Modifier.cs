using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WolfRpc.Analyzer.CodeGen.Methods;

public enum Modifier
{
    [Description("")]
    None = 0,

    [Description("abstract")]
    Abstract = 1,

    [Description("virtual")]
    Virtual = 2,

    [Description("override")]
    Override = 4,

    [Description("static")]
    Static = 8,
}
