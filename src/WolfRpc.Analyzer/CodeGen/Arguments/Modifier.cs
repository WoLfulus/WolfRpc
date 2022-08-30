using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WolfRpc.Analyzer.CodeGen.Arguments;

public enum Modifier
{
    [Description("")]
    None = 0,

    [Description("ref")]
    Ref = 1,

    [Description("out")]
    Out = 2,
}
