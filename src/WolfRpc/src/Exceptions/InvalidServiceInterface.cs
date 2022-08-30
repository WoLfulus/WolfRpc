using System;
using System.Collections.Generic;
using System.Text;

namespace WolfRpc.Exceptions
{
    public class InvalidServiceInterface : Exception
    {
        public InvalidServiceInterface(string type) 
            : base($"Type \"{type}\" is not an interface or doesn't inherit from IService.")
        {
        }
    }
}
