using System.Runtime.CompilerServices;

namespace Kk.LeoQuery
{
    public struct Entity
    {
        internal ISafeEntityOps ops;
        internal SafeEntityId id;

        internal Entity(ISafeEntityOps ops, SafeEntityId id)
        {
            this.ops = ops;
            this.id = id;
        }

        // common methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct
        {
            return ops.Has<T>(id);
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
        public Entity<T> Add<T>(T state) where T : struct
        {
            ops.Add(id, state);
            return new Entity<T>(ops, id);
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
            this.ops = ops;
            this.id = id;
        }
        
        // specialized methods
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T1 Get1()
        {
            return ref ops.Get<T1>(id);
        }

        // common methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct
        {
            return ops.Has<T>(id);
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
        public Entity<T1, T> Add<T>(T state) where T : struct
        {
            ops.Add(id, state);
            return new Entity<T1, T>(ops, id);
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
            this.ops = ops;
            this.id = id;
        }
        
        // specialized methods
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T1 Get1()
        {
            return ref ops.Get<T1>(id);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T2 Get2()
        {
            return ref ops.Get<T2>(id);
        }

        // common methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct
        {
            return ops.Has<T>(id);
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
        public Entity<T2, T> Add<T>(T state) where T : struct
        {
            ops.Add(id, state);
            return new Entity<T2, T>(ops, id);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            ops.Destroy(id);
        }
    }
}