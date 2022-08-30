using System;
using System.Collections.Generic;
using System.Text;
using WolfRpc.Abstractions;

namespace WolfRpc.Tests.Shared;

public interface IMyService : IService
{
    Task<string> Reverse(string value);
}
