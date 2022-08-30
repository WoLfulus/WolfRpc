using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WolfRpc.Analyzer.CodeGen.Methods;

public enum Visibility
{
    [Description("private")]
    Private,

    [Description("protected")]
    Protected,

    [Description("public")]
    Public,

    [Description("")]
    Implicit
}
