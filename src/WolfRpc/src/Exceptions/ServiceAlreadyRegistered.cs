using System;
using System.Collections.Generic;
using System.Text;

namespace WolfRpc.Exceptions
{
    public class ServiceAlreadyRegistered : Exception
    {
        public ServiceAlreadyRegistered() 
            : base($"Service already registered.")
        {
        }

        public ServiceAlreadyRegistered(string name)
            : base($"Service \"{name}\" already registered.")
        {
        }
    }
}
