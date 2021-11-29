using System;

namespace UwU.IFS
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    [PropertyType(typeof(Array))]
    public class GetComponentsInChildren : Attribute
    {
        public bool includeInactive { get; set; }

        public GetComponentsInChildren()
        {
            this.includeInactive = false;
        }

        public GetComponentsInChildren(bool includeInactive)
        {
            this.includeInactive = includeInactive;
        }
    }
}