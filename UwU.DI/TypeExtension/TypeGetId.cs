using System;
using System.Text;
using UwU.Core;

namespace UwU.DI
{
    public static class TypeExtension
    {
        private static TypeId TypeId;
        private static StringBuilder IdBuilder;

        static TypeExtension()
        {
            TypeId = new TypeId();
            IdBuilder = new StringBuilder();
        }

        public static long GetId(this Type type)
        {
            return TypeId.GetId(type);
        }

        public static string GetId(this object obj)
        {
            IdBuilder.Clear();

            var typeID = GetId(obj.GetType());
            IdBuilder.Append(typeID);
            IdBuilder.Append('-');
            IdBuilder.Append(obj.GetHashCode());

            return IdBuilder.ToString();
        }
    }
}