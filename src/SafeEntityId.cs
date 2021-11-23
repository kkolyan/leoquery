using System;
using Leopotam.EcsLite;

namespace Kk.LeoQuery
{
    [Serializable]
    public readonly struct SafeEntityId
    {
        public readonly EcsPackedEntity value;

        public SafeEntityId(EcsPackedEntity value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return $"SafeEntityId({Utils.FieldsToStringByReflection(value)})";
        }
    }
}