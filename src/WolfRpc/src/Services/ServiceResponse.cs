using System;
using System.Collections.Generic;
using System.Text;
using WolfRpc.Abstractions;

namespace WolfRpc.Services;

public class ServiceResponse : IInvokeResponse
{
    public Guid Id { get; private set; }
    public object? Data { get; private set; }
    public Exception? Exception { get; private set; }

    public bool IsSuccess => this.Exception == null;
    public bool IsError => this.Exception != null;

    public ServiceResponse(Guid request, object? result, Exception? exception)
    {
        Id = request;
        Data = result;
        Exception = exception;
    }

    public T? Get<T>()
    {
        if (this.Exception != null)
        {
            throw this.Exception;
        }
        return (T?)this.Data;
    }
}
