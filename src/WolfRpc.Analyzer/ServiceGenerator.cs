using Microsoft.CodeAnalysis;

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
            context.AddSource($"{service.Name}.g.cs", service.ToSource());
        }
    }
}
