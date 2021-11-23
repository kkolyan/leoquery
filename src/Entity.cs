using System.Runtime.CompilerServices;

namespace Kk.LeoQuery
{
    public readonly struct Entity
    {
        private readonly World world;
        private readonly SafeEntityId id;

        internal Entity(World world, SafeEntityId id)
        {
            this.world = world;
            this.id = id;
        }
        
        // debug

        public object[] Components => world.GetComponents(id);

        public override string ToString()
        {
            return $"Entity({Utils.FieldsToStringByReflection(id.value)})";
        }

        // common methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidEntity()
        {
            return world.IsAlive(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct
        {
            return world.Has<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet<T>(out Entity<T> slice) where T : struct
        {
            if (world.Has<T>(id))
            {
                slice = new Entity<T>(world, id);
                return true;
            }

            slice = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>() where T : struct
        {
            return ref world.Get<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Del<T>() where T : struct
        {
            world.Del<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Take<T>() where T : struct
        {
            T foo = world.Get<T>(id);
            world.Del<T>(id);
            return foo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Add<T>() where T : struct
        {
            return ref world.Add<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T>(T state) where T : struct
        {
            world.Add(id, state);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            world.Destroy(id);
        }
    }

    public readonly struct Entity<T1> 
        where T1 : struct
    {
        private readonly World world;
        internal readonly SafeEntityId id;

        internal Entity(World world, SafeEntityId id)
        {
            this.world = world;
            this.id = id;
        }
        
        // debug

        public object[] Components => world.GetComponents(id);

        public override string ToString()
        {
            return $"Entity<{typeof(T1).Name}>({Utils.FieldsToStringByReflection(id.value)})";
        }

        // specialized methods

        public static implicit operator Entity(Entity<T1> entity)
        {
            return new Entity(entity.world, entity.id);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T1 Get1()
        {
            return ref world.Get<T1>(id);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has1()
        {
            return world.Has<T1>(id);
        }

        // common methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidEntity()
        {
            return world.IsAlive(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct
        {
            return world.Has<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet<T>(out Entity<T> slice) where T : struct
        {
            if (world.Has<T>(id))
            {
                slice = new Entity<T>(world, id);
                return true;
            }

            slice = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>() where T : struct
        {
            return ref world.Get<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Take<T>() where T : struct
        {
            T foo = world.Get<T>(id);
            world.Del<T>(id);
            return foo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Del<T>() where T : struct
        {
            world.Del<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Add<T>() where T : struct
        {
            return ref world.Add<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T>(T state) where T : struct
        {
            world.Add(id, state);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            world.Destroy(id);
        }
    }

    public readonly struct Entity<T1, T2> 
        where T1 : struct
        where T2 : struct
    {
        private readonly World world;
        internal readonly SafeEntityId id;

        internal Entity(World world, SafeEntityId id)
        {
            this.world = world;
            this.id = id;
        }
        
        // debug

        public object[] Components => world.GetComponents(id);

        public override string ToString()
        {
            return $"Entity<{typeof(T1).Name},{typeof(T2).Name}>({Utils.FieldsToStringByReflection(id.value)})";
        }
        
        // specialized methods

        public static implicit operator Entity(Entity<T1, T2> entity)
        {
            return new Entity(entity.world, entity.id);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T1 Get1()
        {
            return ref world.Get<T1>(id);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has1()
        {
            return world.Has<T1>(id);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T2 Get2()
        {
            return ref world.Get<T2>(id);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has2()
        {
            return world.Has<T2>(id);
        }

        // common methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidEntity()
        {
            return world.IsAlive(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct
        {
            return world.Has<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet<T>(out Entity<T> slice) where T : struct
        {
            if (world.Has<T>(id))
            {
                slice = new Entity<T>(world, id);
                return true;
            }

            slice = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>() where T : struct
        {
            return ref world.Get<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Take<T>() where T : struct
        {
            T foo = world.Get<T>(id);
            world.Del<T>(id);
            return foo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Del<T>() where T : struct
        {
            world.Del<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Add<T>() where T : struct
        {
            return ref world.Add<T>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T>(T state) where T : struct
        {
            world.Add(id, state);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            world.Destroy(id);
        }
    }
}