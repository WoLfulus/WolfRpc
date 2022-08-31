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

    public static string Indent(string content, int size = 0, string with = "    ")
    {
        var lines = content.Split('\n');
        var tab = String.Join("", Enumerable.Range(0, size).Select(_ => with).ToArray());
        var indented = tab + Join(lines, '\n' + tab);
        return String.Join("\r\n", indented.Split('\n').Select(line => line.TrimEnd()).ToArray());
    }
}
