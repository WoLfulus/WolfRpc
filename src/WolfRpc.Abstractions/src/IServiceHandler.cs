namespace WolfRpc.Abstractions;

public interface IServiceHandler : IService, IDisposable
{
    ulong Id { get; }

    public IInvokeResponse Invoke(IInvokeRequest invoke);
    public Task<IInvokeResponse> InvokeAsync(IInvokeRequest invoke);
}
