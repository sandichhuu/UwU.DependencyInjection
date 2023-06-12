using System;
using System.Collections.Generic;

namespace UwU.TypeId
{
    public class IdProvider
    {
        private readonly Dictionary<Type, int> indexCache;
        private readonly List<Type> typeCache;

        private int currentId;

        public IdProvider()
        {
            this.currentId = 0;
            this.indexCache = new(16);
            this.typeCache = new(16);
        }

        public int GetId<Type>()
        {
            int index;
            var type = typeof(Type);

            if (this.indexCache.TryGetValue(type, out var t))
            {
                index = t;
            }
            else
            {
                this.typeCache.Add(type);
                this.indexCache.Add(type, this.currentId);
                index = this.currentId;
                this.currentId++;
            }

            return index;
        }

        public int GetId(Type type)
        {
            int index;

            if (this.indexCache.TryGetValue(type, out var t))
            {
                index = t;
            }
            else
            {
                this.typeCache.Add(type);
                this.indexCache.Add(type, this.currentId);
                index = this.currentId;
                this.currentId++;
            }

            return index;
        }

        public Type GetTypeFromID(int id)
        {
            return this.typeCache[id];
        }
    }
}