using System.Diagnostics.CodeAnalysis;
using WolfRpc.Abstractions;
using WolfRpc.Services;

namespace WolfRpc;

public abstract class Service<I> : IServiceHandler
{
    private static readonly ulong id = WolfRpc.Identifier.From(typeof(I).FullName);
    public virtual ulong Id => id;

    public Task<T> GetAddressable<T>(string address) where T : IAddressable
    {
        throw new NotImplementedException();
    }

    public virtual IInvokeResponse Invoke(IInvokeRequest invoke)
    {
        var task = this.InvokeAsync(invoke);
        task.Wait();
        return task.Result;
    }

    public abstract Task<IInvokeResponse> InvokeAsync(IInvokeRequest invoke);

    protected object? GetNullableTaskResult(Task task)
    {
        var taskType = task.GetType();
        if (taskType.IsGenericType)
        {
            return taskType.GetProperty(nameof(Task<object>.Result)).GetValue(task, null);
        }

        return null;
    }

    public virtual void Dispose()
    {
    }
}