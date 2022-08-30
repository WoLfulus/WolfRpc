using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using WolfRpc.Analyzer.Extensions;
using WolfRpc.CodeGen;
using Microsoft.CodeAnalysis.CSharp;
using WolfRpc.Analyzer.CodeGen.Methods;
using WolfRpc.CodeGen.Arguments;

namespace WolfRpc.Analyzer
{
    internal class ServiceGeneratorData : ISyntaxContextReceiver
    {
        public IList<ServiceHandler> ServiceHandlers { get; } = new List<ServiceHandler>();

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is InterfaceDeclarationSyntax declaration)
            {
                var symbol = context.SemanticModel.GetDeclaredSymbol(declaration) as ITypeSymbol;
                foreach (var interfaceType in symbol!.Interfaces)
                {
                    var name = interfaceType.GetFullName();
                    if (name != "WolfRpc.Abstractions.IService")
                    {
                        continue;
                    }

                    var service = new ServiceHandler()
                    {
                        Name = symbol.Name,
                        Namespace = symbol.GetNamespace(),

                    };

                    foreach (var method in declaration.Members.OfType<MethodDeclarationSyntax>())
                    {
                        service.Methods.Add(new Method(
                            method.ReturnType.ToString(),
                            method.Identifier.Text,
                            method.ParameterList.Parameters.Select(x => new Argument()
                            {
                                Name = x.Identifier.Text,
                                Type = x.Type!.ToString(),
                            }).ToList(),
                            Visibility.Public, 
                            Modifier.Abstract
                        ));
                    }

                    ServiceHandlers.Add(service);
                }
            }
        }
    }
}
