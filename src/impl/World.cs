using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Kk.LeoQuery
{
    internal readonly struct World
    {
        internal readonly EcsWorld raw;
        internal readonly Dictionary<Type, object> filters;

        // for debug watches
        internal Entity[] Entities
        {
            get
            {
                int[] entities = null;
                raw.GetAllEntities(ref entities);
                Entity[] wrapped = new Entity[entities.Length];
                for (var i = 0; i < entities.Length; i++)
                {
                    wrapped[i] = new Entity(raw.PackEntityWithWorld(entities[i]));
                }
                return wrapped;
            }
        }

        public World(EcsWorld raw)
        {
            this.raw = raw;
            filters = new Dictionary<Type, object>();
        }
    }
}