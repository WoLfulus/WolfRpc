using System;

namespace WolfRpc.Abstractions;

public interface IAddressable : IAddressableLifetime
{
    string Address { get; }
}
