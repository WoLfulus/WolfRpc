using System.IO;
using System.Reflection;
using System.Text;

using Microsoft.CodeAnalysis.Text;

namespace WolfRpc.Analyzer.Test.Utilities;

public static class Data
{
    public static string Read(string name)
    {
        return Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, File.ReadAllBytes($"data\\{name}")));
    }

    public static (string, SourceText) ReadWithName(string name)
    {
        var content = Read(name);

        var header = "// filename: ";
        if (content.StartsWith(header))
        {
            var lines = content.Split('\n').ToList();

            // 1st line
            name = lines[0].Substring(header.Length).Trim();
            lines.RemoveAt(0);

            if (lines.Count > 0)
            {
                if (lines[0].Trim() == "")
                {
                    lines.RemoveAt(0);
                }
            }
            content = String.Join('\n', lines);
        }

        return (name, SourceText.From(content, Encoding.UTF8, SourceHashAlgorithm.Sha256));
    }
}