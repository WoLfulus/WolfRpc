using System.Reflection;
using WolfRpc.Abstractions;
using WolfRpc.Exceptions;

namespace WolfRpc.Services;

public class ServiceCollection
{
    private Dictionary<ulong, IServiceHandler> services = new Dictionary<ulong, IServiceHandler>();

    public ServiceCollection Add<T, V>(V service)
        where T : IService
        where V : T, IServiceHandler
    {
        var serviceId = service.Id;
        if (this.services.ContainsKey(serviceId))
        {
            throw new ServiceAlreadyRegistered();
        }

        var type = typeof(T);
        if (!type.IsInterface || type.IsAssignableFrom(typeof(IService)))
        {
            throw new InvalidServiceInterface(type.FullName);
        }

        this.services[serviceId] = service;
        return this;
    }

    public IServiceHandler? Find(ulong service)
    {
        if (this.services.TryGetValue(service, out var serviceHandler))
        {
            return serviceHandler;
        }
        return null;
    }
    
    public void Dispose()
    {
        foreach (var service in this.services.Values)
        {
            service.Dispose();
        }
    }
}