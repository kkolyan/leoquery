using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Kk.LeoQuery
{
    internal readonly struct World
    {
        internal readonly EcsWorld raw;
        internal readonly Dictionary<Type, object> filters;

        public World(EcsWorld raw)
        {
            this.raw = raw;
            filters = new Dictionary<Type, object>();
        }
    }
}