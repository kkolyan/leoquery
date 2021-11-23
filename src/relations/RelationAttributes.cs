using System;
using System.Collections.Generic;
using System.Reflection;

namespace Kk.LeoQuery
{
    public static class RelationAttributes
    {

        private static Dictionary<Type, IRelationsConfig> _relationConfigByAttr = new Dictionary<Type, IRelationsConfig>();
        
        public static void RegisterRelationAttribute<TAttr, TRelationConfig>()
            where TAttr : Attribute
            where TRelationConfig : IRelationsConfig, new()
        {
            Type attrType = typeof(TAttr);
            if (_relationConfigByAttr.TryGetValue(attrType, out IRelationsConfig existing) && existing.GetType() != typeof(TRelationConfig))
            {
                throw new Exception($"duplicate relation config for attribute {attrType.FullName}");
            }
            _relationConfigByAttr[attrType] = new TRelationConfig();
        }

        internal static bool TryGet(Type attributedType, out IRelationsConfig result)
        {
            foreach (Attribute attribute in attributedType.GetCustomAttributes())
            {
                if (_relationConfigByAttr.TryGetValue(attribute.GetType(), out result))
                {
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}