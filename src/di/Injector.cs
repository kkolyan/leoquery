using System;
using System.Collections.Generic;
using System.Reflection;

namespace Kk.LeoQuery
{
    public class Injector
    {
        private readonly Dictionary<Type, object> _dependencies = new Dictionary<Type, object>();
        
        

        public Injector AddDependency(object o, Type overridenType = null)
        {
            Type type = overridenType ?? o.GetType();
            if (_dependencies.ContainsKey(type))
            {
                throw new Exception($"duplicate key: {type}");
            }

            _dependencies[type] = o;
            return this;
        }

        public void InjectInto(ISystem system)
        {
            foreach (FieldInfo field in system.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (field.GetCustomAttribute<Inject>() != null)
                {
                    if (!_dependencies.TryGetValue(field.FieldType, out var dep))
                    {
                        throw new Exception($"failed to resolve dependency for {field}");
                    }

                    field.SetValue(system, dep);
                }
            }
        }
    }
}