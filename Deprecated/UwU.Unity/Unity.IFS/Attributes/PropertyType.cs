using System;

namespace UwU.IFS
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PropertyTypeAttribute : Attribute
    {
        public Type[] types { get; private set; }

        public PropertyTypeAttribute(params Type[] types)
        {
            this.types = types;
        }
    }
}