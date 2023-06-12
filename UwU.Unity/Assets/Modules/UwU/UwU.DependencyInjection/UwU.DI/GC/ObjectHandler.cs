using System;
using System.Runtime.InteropServices;

namespace UwU.DI.GC
{
    public class ObjectHandler : IDisposable
    {
        public readonly GCHandle handle;
        public IntPtr pointer => this.handle.ToIntPtr();

        public ObjectHandler(object obj)
        {
            this.handle = obj.ToGcHandle();
        }

        public void Dispose()
        {
            this.handle.Free();
        }
    }
}