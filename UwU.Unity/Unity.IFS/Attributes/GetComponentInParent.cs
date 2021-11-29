using System;

namespace UwU.IFS
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GetComponentInParent : Attribute
    {
    }
}