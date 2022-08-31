using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

using WolfRpc.Abstractions;
using WolfRpc.Analyzer.Test.Utilities;

namespace WolfRpc.Analyzer.Test;

public class TestGenerator
{
    [Fact]
    public async void TestSourceGenerator2()
    {
        var input = Data.Read("IMyService.cs");
        var output = Data.ReadWithName("IMyService.g.cs");

        var results = TestGeneration(input);

        Assert.Single(results.Results);
        Assert.Equal(output.Item1, results.Results[0].GeneratedSources[0].HintName);
        Assert.Equal(output.Item2.Source(), results.Results[0].GeneratedSources[0].SourceText.Source(), false, true, true);
    }

    private static GeneratorDriverRunResult TestGeneration(string input)
    {
        var inputCompilation = CSharpCompilation.Create(
            "compilation",
            new[] { CSharpSyntaxTree.ParseText(input) },
            new[] {
                MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Service<>).Assembly.Location), 
                MetadataReference.CreateFromFile(typeof(IService).Assembly.Location),
            },
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

            
        var generator = new ServiceGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

        return driver.GetRunResult();        
    }
}
