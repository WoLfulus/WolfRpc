using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace WolfRpc.Analyzer;

[Generator]
public class ServiceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new ServiceGeneratorData());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var data = context.SyntaxContextReceiver as ServiceGeneratorData;
        foreach (var service in data!.ServiceHandlers)
        {
            context.AddSource($"{service.FileName}", SourceText.From(service.ToSource(), Encoding.UTF8, SourceHashAlgorithm.Sha256));
        }
    }
}
