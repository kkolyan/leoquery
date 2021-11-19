namespace Kk.LeoQuery
{
    internal interface ISafeEntityOps
    {
        ref T Get<T>(SafeEntityId id) where T : struct;

        bool Has<T>(SafeEntityId id) where T : struct;

        void Add<T>(SafeEntityId id, T state) where T : struct;

        void Del<T>(SafeEntityId id) where T : struct;

        void Destroy(SafeEntityId id);
    }
}