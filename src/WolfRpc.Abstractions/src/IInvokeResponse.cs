using System;

namespace WolfRpc.Abstractions;

public interface IInvokeResponse
{
    public Guid Id { get; }
    public object? Data { get; }
    public Exception? Exception { get; }

    public T? Get<T>();
    
    public bool IsSuccess { get; }
    public bool IsError { get; }
}
