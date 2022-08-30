using System;
using System.Collections.Generic;
using System.Text;
using WolfRpc.Abstractions;
using WolfRpc.Tests.Shared;
using WolfRpc.Tests.Shared.Generated;

namespace WolfRpc.Tests.Services;

public class MyService : MyServiceHandler
{
    public override Task<string> Reverse(string value)
    {
        var reversed = new string(value.Reverse().ToArray());
        return Task.FromResult(reversed);
    }
}
