using System;
using System.Collections.Generic;
using System.Reflection;

namespace Kk.LeoQuery
{
    public static class RelationAttributes
    {

        private static Dictionary<Type, DescribeRelations> _relationConfigByAttr = new Dictionary<Type, DescribeRelations>();
        
        public static void Register<TAttr>(DescribeRelations relations)
            where TAttr : Attribute
        {
            Type attrType = typeof(TAttr);
            if (_relationConfigByAttr.ContainsKey(attrType))
            {
                throw new Exception($"duplicate relation config for attribute {attrType.FullName}");
            }
            _relationConfigByAttr[attrType] = relations;
        }

        internal static bool TryGet(Type attributedType, out DescribeRelations result)
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