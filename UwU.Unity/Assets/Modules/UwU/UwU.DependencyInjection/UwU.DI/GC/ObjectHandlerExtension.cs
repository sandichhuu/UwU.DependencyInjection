using System;
using System.Runtime.InteropServices;

namespace UwU.DI.GC
{
    public static class ObjectHandleExtensions
    {
        public static IntPtr ToIntPtr(this object target)
        {
            return GCHandle.Alloc(target).ToIntPtr();
        }

        public static GCHandle ToGcHandle(this object target)
        {
            return GCHandle.Alloc(target);
        }

        public static IntPtr ToIntPtr(this GCHandle target)
        {
            return GCHandle.ToIntPtr(target);
        }

        public static GCHandle ToGcHandle(this IntPtr pointer)
        {
            return GCHandle.FromIntPtr(pointer);
        }
    }
}