using WolfRpc.Abstractions;

namespace WolfRpc.Objects;

public class AddressableCollection : IAddressableCollection
{
    public delegate Task<IAddressable> CreateRemoteObjectDelegate(string id);

    private readonly Dictionary<Type, CreateRemoteObjectDelegate> objects = new Dictionary<Type, CreateRemoteObjectDelegate>();

    public void Add<T, V>()
        where T : IAddressable
        where V : T, new()
    {
        objects[typeof(T)] = async (id) => {
            var obj = (V)Activator.CreateInstance(typeof(V), new object[] { id });
            await obj.OnActivate();
            return obj;
        };
    }    
}