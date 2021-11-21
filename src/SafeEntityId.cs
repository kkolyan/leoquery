using System;
using Leopotam.EcsLite;

namespace Kk.LeoQuery
{
    [Serializable]
    public struct SafeEntityId
    {
        public EcsPackedEntity value;

        public override string ToString()
        {
            return $"SafeEntityId({Utils.FieldsToStringByReflection(value)})";
        }
    }
}