namespace Kk.LeoQuery
{
    internal interface ISafeEntityOps
    {
        ref T Get<T>(SafeEntityId id) where T : struct;

        bool Has<T>(SafeEntityId id) where T : struct;

        ref T Add<T>(SafeEntityId id) where T : struct;
        
        void Add<T>(SafeEntityId id, T initialState) where T : struct;

        void Del<T>(SafeEntityId id) where T : struct;

        void Destroy(SafeEntityId id);

        bool IsAlive(SafeEntityId id);

        object[] GetComponents(SafeEntityId id);
    }
}