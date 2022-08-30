using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WolfRpc.Analyzer.CodeGen.Source;

public static class SourceString
{
    public static string Join(object[] blocks, string separator = " ")
    {
        return string.Join(separator,
            blocks
                .Select(block =>
                {
                    if (block is string stringBlock)
                    {
                        return stringBlock;
                    }
                    else if (block is ISourceBlock sourceBlockInterface)
                    {
                        return sourceBlockInterface.ToSource();
                    }
                    else if (block is SourceBlock sourceBlock)
                    {
                        return sourceBlock.ToSource();
                    }
                    else
                    {
                        return block.ToString();
                    }
                })
                .Where(block => block != null && block != "")
        );
    }


    public static string JoinWith(string separator, object[] blocks)
    {
        return Join(blocks, separator);
    }

    public static string Indent(string content, int size = 0, char with = '\t')
    {
        var lines = content.Split('\n');
        var tab = new string(with, size);

        return tab + Join(lines, tab);
    }
}
