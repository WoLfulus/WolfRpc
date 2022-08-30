using WolfRpc.Abstractions;

namespace WolfRpc.Objects
{
    public class Addressable : IAddressable, IAddressableLifetime
    {
        public string Address { get; private set; }

        protected Addressable()
        {
            Address = "";
        }

        protected Addressable(string id)
        {
            Address = id;
        }

        public virtual Task OnActivate()
        {
            return Task.CompletedTask;
        }

        public virtual Task OnDestroy()
        {
            return Task.CompletedTask;
        }
    }
}