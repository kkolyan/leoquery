namespace Kk.LeoQuery
{
    public interface IEntityStorage
    {
        IEntitySet<T> Query<T>() where T : struct;
        IEntitySet<T> Query<T>(int worldIndex) where T : struct;

        IEntitySet<T1, T2> Query<T1, T2>()
            where T1 : struct
            where T2 : struct;
        
        IEntitySet<T1, T2> Query<T1, T2>(int worldIndex)
            where T1 : struct
            where T2 : struct;

        bool TrySingle<T>(out Entity<T> entity) where T : struct;
        bool TrySingle<T>(int worldIndex, out Entity<T> entity) where T : struct;
        
        Entity NewEntity();
        Entity NewEntity(int worldIndex);
    }
}