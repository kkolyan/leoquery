using System;
using System.Runtime.CompilerServices;

namespace Kk.LeoQuery
{
    public struct Entity
    {
        internal ISafeEntityOps ops;
        internal SafeEntityId id;

        internal Entity(ISafeEntityOps ops, SafeEntityId id)
        {
            this.ops = ops ?? throw new Exception("ops is null");
            this.id = id;
        }
        
        // debug

        public object[] Components => ops.GetComponents(id);

        public override string ToString()
        {
            return $"Entity({Utils.FieldsToStringByReflection(id.value)})";
        }

        // common methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidEntity()
        {
            return ops.IsAlive(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct
        {
            return ops.Has<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet<T>(out Entity<T> slice) where T : struct
        {
            if (ops.Has<T>(id))
            {
                slice = new Entity<T>(ops, id);
                return true;
            }

            slice = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>() where T : struct
        {
            return ref ops.Get<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Del<T>() where T : struct
        {
            ops.Del<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Take<T>() where T : struct
        {
            T foo = ops.Get<T>(id);
            ops.Del<T>(id);
            return foo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Add<T>() where T : struct
        {
            return ref ops.Add<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T>(T state) where T : struct
        {
            ops.Add(id, state);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            ops.Destroy(id);
        }
    }

    public struct Entity<T1> 
        where T1 : struct
    {
        internal ISafeEntityOps ops;
        internal SafeEntityId id;

        internal Entity(ISafeEntityOps ops, SafeEntityId id)
        {
            this.ops = ops ?? throw new Exception("ops is null");
            this.id = id;
        }
        
        // debug

        public object[] Components => ops.GetComponents(id);

        public override string ToString()
        {
            return $"Entity<{typeof(T1).Name}>({Utils.FieldsToStringByReflection(id.value)})";
        }

        // specialized methods

        public static implicit operator Entity(Entity<T1> entity)
        {
            return new Entity(entity.ops, entity.id);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T1 Get1()
        {
            return ref ops.Get<T1>(id);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has1()
        {
            return ops.Has<T1>(id);
        }

        // common methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidEntity()
        {
            return ops.IsAlive(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct
        {
            return ops.Has<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet<T>(out Entity<T> slice) where T : struct
        {
            if (ops.Has<T>(id))
            {
                slice = new Entity<T>(ops, id);
                return true;
            }

            slice = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>() where T : struct
        {
            return ref ops.Get<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Take<T>() where T : struct
        {
            T foo = ops.Get<T>(id);
            ops.Del<T>(id);
            return foo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Del<T>() where T : struct
        {
            ops.Del<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Add<T>() where T : struct
        {
            return ref ops.Add<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T>(T state) where T : struct
        {
            ops.Add(id, state);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            ops.Destroy(id);
        }
    }

    public struct Entity<T1, T2> 
        where T1 : struct
        where T2 : struct
    {
        internal ISafeEntityOps ops;
        internal SafeEntityId id;

        internal Entity(ISafeEntityOps ops, SafeEntityId id)
        {
            this.ops = ops ?? throw new Exception("ops is null");
            this.id = id;
        }
        
        // debug

        public object[] Components => ops.GetComponents(id);

        public override string ToString()
        {
            return $"Entity<{typeof(T1).Name},{typeof(T2).Name}>({Utils.FieldsToStringByReflection(id.value)})";
        }
        
        // specialized methods

        public static implicit operator Entity(Entity<T1, T2> entity)
        {
            return new Entity(entity.ops, entity.id);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T1 Get1()
        {
            return ref ops.Get<T1>(id);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has1()
        {
            return ops.Has<T1>(id);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T2 Get2()
        {
            return ref ops.Get<T2>(id);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has2()
        {
            return ops.Has<T2>(id);
        }

        // common methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidEntity()
        {
            return ops.IsAlive(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct
        {
            return ops.Has<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet<T>(out Entity<T> slice) where T : struct
        {
            if (ops.Has<T>(id))
            {
                slice = new Entity<T>(ops, id);
                return true;
            }

            slice = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>() where T : struct
        {
            return ref ops.Get<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Take<T>() where T : struct
        {
            T foo = ops.Get<T>(id);
            ops.Del<T>(id);
            return foo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Del<T>() where T : struct
        {
            ops.Del<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Add<T>() where T : struct
        {
            return ref ops.Add<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T>(T state) where T : struct
        {
            ops.Add(id, state);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            ops.Destroy(id);
        }
    }
}