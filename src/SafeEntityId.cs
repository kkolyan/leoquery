using System;
using Leopotam.EcsLite;

namespace Kk.LeoQuery
{
    [Serializable]
    public struct SafeEntityId
    {
        public EcsPackedEntity value;
    }
}