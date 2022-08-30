using System;
using System.Collections.Generic;
using System.Text;
using WolfRpc.Abstractions;

namespace WolfRpc.Services;

public class ServiceRequest : IInvokeRequest
{
    public Guid Id { get; set; }
    public ulong Service { get; set; }
    public ulong Method { get; set; }
    public object[] Arguments { get; set; }

    public ServiceRequest(Guid id, ulong service, ulong method, object[] args)
    {
        Id = id;
        Service = service;
        Method = method;
        Arguments = args;
    }

    public ServiceRequest(ulong service, ulong method, object[] args)
    {
        Id = Guid.NewGuid();
        Service = service;
        Method = method;
        Arguments = args;
    }
}
