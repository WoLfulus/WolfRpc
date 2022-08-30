namespace WolfRpc.Abstractions;

public interface IInvokeRequest
{
    public Guid Id { get; }
    public ulong Service { get; }
    public ulong Method { get; }
    public object[] Arguments { get; }
}
