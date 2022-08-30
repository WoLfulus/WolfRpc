using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis;

namespace WolfRpc.Analyzer.Extensions;

public static class ITypeSymbolExtensions
{
    public static string GetFullName(this ITypeSymbol symbol)
    {
        var currentNamespace = symbol.ContainingNamespace;
        
        var parts = new List<string>(new string[] { symbol.Name });
        while (currentNamespace != null && !currentNamespace.IsGlobalNamespace)
        {
            parts.Add(currentNamespace.Name);
            currentNamespace = currentNamespace.ContainingNamespace;
        }
        
        parts.Reverse();
        
        return String.Join(".", parts);
    }

    public static string GetName(this ITypeSymbol symbol)
    {
        return symbol.Name;
    }

    public static string GetNamespace(this ITypeSymbol symbol)
    {
        var currentNamespace = symbol.ContainingNamespace;

        var parts = new List<string>();
        while (currentNamespace != null && !currentNamespace.IsGlobalNamespace)
        {
            parts.Add(currentNamespace.Name);
            currentNamespace = currentNamespace.ContainingNamespace;
        }

        parts.Reverse();

        return String.Join(".", parts);
    }
}
