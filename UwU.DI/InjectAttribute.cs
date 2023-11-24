using System;

namespace UwU.DI
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
        AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class Inject : Attribute
    {
        public Inject()
        {
        }
    }
}