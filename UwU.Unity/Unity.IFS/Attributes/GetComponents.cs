using System;

namespace UwU.IFS
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    [PropertyType(typeof(Array))]
    public class GetComponents : Attribute
    {
    }
}