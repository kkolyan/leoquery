using System.Runtime.CompilerServices;
using Leopotam.EcsLite;

namespace Kk.LeoQuery
{
    public readonly struct Entity
    {
        public readonly EcsPackedEntityWithWorld raw;

        public Entity(EcsPackedEntityWithWorld raw)
        {
            this.raw = raw;
        }

        // debug

        public object[] Components => raw.GetComponents();

        public override string ToString()
        {
            return $"Entity({Utils.FieldsToStringByReflection(raw)})";
        }

        // common methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidEntity()
        {
            return raw.IsAlive();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct
        {
            return raw.Has<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet<T>(out Entity<T> slice) where T : struct
        {
            if (raw.Has<T>())
            {
                slice = new Entity<T>(raw);
                return true;
            }

            slice = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>() where T : struct
        {
            return ref raw.Get<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Del<T>() where T : struct
        {
            raw.Del<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Take<T>() where T : struct
        {
            T foo = raw.Get<T>();
            raw.Del<T>();
            return foo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Add<T>() where T : struct
        {
            return ref raw.Add<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T>(T state) where T : struct
        {
            raw.Add(state);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            raw.Destroy();
        }
    }

    public readonly struct Entity<T1>
        where T1 : struct
    {
        public readonly EcsPackedEntityWithWorld id;

        public Entity(EcsPackedEntityWithWorld id)
        {
            this.id = id;
        }

        // debug

        public object[] Components => id.GetComponents();

        public override string ToString()
        {
            return $"Entity<{typeof(T1).Name}>({Utils.FieldsToStringByReflection(id)})";
        }

        // specialized methods

        public static implicit operator Entity(Entity<T1> entity)
        {
            return new Entity(entity.id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T1 Get1()
        {
            return ref id.Get<T1>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has1()
        {
            return id.Has<T1>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Del1()
        {
            id.Del<T1>();
        }

        // common methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidEntity()
        {
            return id.IsAlive();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct
        {
            return id.Has<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet<T>(out Entity<T> slice) where T : struct
        {
            if (id.Has<T>())
            {
                slice = new Entity<T>();
                return true;
            }

            slice = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet<T1, T2>(out Entity<T1, T2> slice)
            where T1 : struct
            where T2 : struct
        {
            if (id.Has<T1>() && id.Has<T2>())
            {
                slice = new Entity<T1, T2>();
                return true;
            }

            slice = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet<T1, T2, T3>(out Entity<T1, T2, T3> slice)
            where T1 : struct
            where T2 : struct
            where T3 : struct
        {
            if (id.Has<T1>() && id.Has<T2>() && id.Has<T3>())
            {
                slice = new Entity<T1, T2, T3>();
                return true;
            }

            slice = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>() where T : struct
        {
            return ref id.Get<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Take<T>() where T : struct
        {
            T foo = id.Get<T>();
            id.Del<T>();
            return foo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Del<T>() where T : struct
        {
            id.Del<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Add<T>() where T : struct
        {
            return ref id.Add<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T>(T state) where T : struct
        {
            id.Add(state);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            id.Destroy();
        }
    }

    public readonly struct Entity<T1, T2>
        where T1 : struct
        where T2 : struct
    {
        public readonly EcsPackedEntityWithWorld raw;

        public Entity(EcsPackedEntityWithWorld raw)
        {
            this.raw = raw;
        }

        // debug

        public object[] Components => raw.GetComponents();

        public override string ToString()
        {
            return $"Entity<{typeof(T1).Name},{typeof(T2).Name}>({Utils.FieldsToStringByReflection(raw)})";
        }

        // specialized methods

        public static implicit operator Entity(Entity<T1, T2> entity)
        {
            return new Entity(entity.raw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T1 Get1()
        {
            return ref raw.Get<T1>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has1()
        {
            return raw.Has<T1>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Del1()
        {
            raw.Del<T1>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T2 Get2()
        {
            return ref raw.Get<T2>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has2()
        {
            return raw.Has<T2>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Del2()
        {
            raw.Del<T2>();
        }

        // common methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidEntity()
        {
            return raw.IsAlive();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct
        {
            return raw.Has<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet<T>(out Entity<T> slice) where T : struct
        {
            if (raw.Has<T>())
            {
                slice = new Entity<T>(raw);
                return true;
            }

            slice = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet<T1, T2>(out Entity<T1, T2> slice)
            where T1 : struct
            where T2 : struct
        {
            if (raw.Has<T1>() && raw.Has<T2>())
            {
                slice = new Entity<T1, T2>();
                return true;
            }

            slice = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet<T1, T2, T3>(out Entity<T1, T2, T3> slice)
            where T1 : struct
            where T2 : struct
            where T3 : struct
        {
            if (raw.Has<T1>() && raw.Has<T2>() && raw.Has<T3>())
            {
                slice = new Entity<T1, T2, T3>();
                return true;
            }

            slice = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>() where T : struct
        {
            return ref raw.Get<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Take<T>() where T : struct
        {
            T foo = raw.Get<T>();
            raw.Del<T>();
            return foo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Del<T>() where T : struct
        {
            raw.Del<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Add<T>() where T : struct
        {
            return ref raw.Add<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T>(T state) where T : struct
        {
            raw.Add(state);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            raw.Destroy();
        }
    }

    public readonly struct Entity<T1, T2, T3>
        where T1 : struct
        where T2 : struct
        where T3 : struct
    {
        public readonly EcsPackedEntityWithWorld raw;

        public Entity(EcsPackedEntityWithWorld raw)
        {
            this.raw = raw;
        }

        // debug

        public object[] Components => raw.GetComponents();

        public override string ToString()
        {
            return $"Entity<{typeof(T1).Name},{typeof(T2).Name},{typeof(T3).Name}>({Utils.FieldsToStringByReflection(raw)})";
        }

        // specialized methods

        public static implicit operator Entity(Entity<T1, T2, T3> entity)
        {
            return new Entity(entity.raw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T1 Get1()
        {
            return ref raw.Get<T1>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has1()
        {
            return raw.Has<T1>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Del1()
        {
            raw.Del<T1>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T2 Get2()
        {
            return ref raw.Get<T2>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has2()
        {
            return raw.Has<T2>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Del2()
        {
            raw.Del<T2>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T3 Get3()
        {
            return ref raw.Get<T3>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has3()
        {
            return raw.Has<T3>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Del3()
        {
            raw.Del<T3>();
        }

        // common methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidEntity()
        {
            return raw.IsAlive();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has<T>() where T : struct
        {
            return raw.Has<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet<T>(out Entity<T> slice) where T : struct
        {
            if (raw.Has<T>())
            {
                slice = new Entity<T>();
                return true;
            }

            slice = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet<T1, T2>(out Entity<T1, T2> slice)
            where T1 : struct
            where T2 : struct
        {
            if (raw.Has<T1>() && raw.Has<T2>())
            {
                slice = new Entity<T1, T2>();
                return true;
            }

            slice = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet<T1, T2, T3>(out Entity<T1, T2, T3> slice)
            where T1 : struct
            where T2 : struct
            where T3 : struct
        {
            if (raw.Has<T1>() && raw.Has<T2>() && raw.Has<T3>())
            {
                slice = new Entity<T1, T2, T3>();
                return true;
            }

            slice = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>() where T : struct
        {
            return ref raw.Get<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Take<T>() where T : struct
        {
            T foo = raw.Get<T>();
            raw.Del<T>();
            return foo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Del<T>() where T : struct
        {
            raw.Del<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Add<T>() where T : struct
        {
            return ref raw.Add<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity Add<T>(T state) where T : struct
        {
            raw.Add(state);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            raw.Destroy();
        }
    }
}