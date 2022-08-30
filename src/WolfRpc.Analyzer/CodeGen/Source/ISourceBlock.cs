using System;
using System.Collections.Generic;
using System.Text;

namespace WolfRpc.Analyzer.CodeGen.Source;

public interface ISourceBlock
{
    string ToSource();
    string ToSource(int indent);
}
