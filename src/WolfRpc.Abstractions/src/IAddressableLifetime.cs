using System;

namespace WolfRpc.Abstractions;

public interface IAddressableLifetime
{
    Task OnActivate();
    Task OnDestroy();
}
