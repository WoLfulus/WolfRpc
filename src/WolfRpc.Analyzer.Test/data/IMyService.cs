// filename: IMyService.cs

using System;
using WolfRpc.Abstractions;

namespace TestApp.Services
{
    public interface IMyService : IService
    {
        Task<string> Hello();
        Task<string> Hello(string who);
    }
}
