namespace Kk.LeoQuery
{
    public interface IEntityStorage
    {
        IEntitySet<T> Query<T>() where T : struct;

        bool TrySingle<T>(out Entity<T> entity) where T : struct;

        IEntitySet<T1, T2> Query<T1, T2>()
            where T1 : struct
            where T2 : struct;

        Entity NewEntity();
    }
}