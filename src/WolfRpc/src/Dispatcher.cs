
using System;
using WolfRpc.Abstractions;
using WolfRpc.Objects;
using WolfRpc.Services;

namespace WolfRpc;

public class Dispatcher
{
    private ServiceCollection services = new ServiceCollection();
    public ServiceCollection Services => services;

    private AddressableCollection addressables = new AddressableCollection();
    public IAddressableCollection Addressables => addressables;

    public void AddService<T, V>(V service)
        where T : IService
        where V : T, IServiceHandler
    {
        this.services.Add<T, V>(service);
    }

    public void AddService<T, V>()
        where T : IService
        where V : T, IServiceHandler, new()
    {
        this.services.Add<T, V>(new V());
    }

    public void AddAddressable<T, V>()
        where T : IAddressable
        where V : T, new()
    {
        this.addressables.Add<T, V>();
    }

    public async Task<IInvokeResponse> Process(IInvokeRequest invoke)
    {
        var service = this.services.Find(invoke.Service);
        if (service == null)
        {
            return new ServiceResponse(invoke.Id, null, new InvalidOperationException($"Service not registered"));
        }

        return await service.InvokeAsync(invoke);
    }
}