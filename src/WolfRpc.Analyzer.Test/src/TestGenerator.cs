using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

using WolfRpc.Abstractions;

namespace WolfRpc.Analyzer.Test;

public class TestGenerator
{
    [Fact]
    public async void TestSourceGenerator()
    {
        var tester = new CSharpSourceGeneratorTest<ServiceGenerator, XUnitVerifier>
        {
            TestState =
            {
                Sources = { @"
using System;
using WolfRpc.Abstractions;

namespace WolfApp.Services
{
    public interface IMyService : IService
    {
        Task<string> Hello();
        Task<string> Hello(string who);
    }
}
" },
            }
        };
        tester.TestState.GeneratedSources.Add(("WolfRpc.g.cs", ""));
        tester.TestState.AdditionalReferences.Add(typeof(IService).Assembly);
        tester.TestState.AdditionalReferences.Add(typeof(Service<>).Assembly);
        await tester.RunAsync();
    }
}
