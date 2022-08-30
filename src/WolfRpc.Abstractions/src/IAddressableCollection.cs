using System;

namespace WolfRpc.Abstractions;

public interface IAddressableCollection
{
    void Add<T, V>()
        where T : IAddressable
        where V : T, new();
}
