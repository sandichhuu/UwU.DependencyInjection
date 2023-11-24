using System;
using System.Runtime.InteropServices;

namespace UwU.DI.GC
{
    public class ObjectHandler : IDisposable
    {
        public readonly GCHandle handle;

        public IntPtr pointer
        {
            get
            {
                return handle.ToIntPtr();
            }
        }

        public ObjectHandler(object obj)
        {
            handle = obj.ToGcHandle();
        }

        public void Dispose()
        {
            handle.Free();
        }
    }
}