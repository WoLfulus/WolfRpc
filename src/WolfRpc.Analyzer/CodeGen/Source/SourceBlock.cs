using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WolfRpc.Analyzer.CodeGen.Source;

public abstract class SourceBlock : ISourceBlock
{
    public abstract string ToSource();
    
    public virtual string ToSource(int indent = 0)
    {
        return SourceString.Indent(this.ToSource(), indent, '\t');
    }

    public override string ToString()
    {
        return ToSource();
    }
}
