namespace Kk.LeoQuery
{
    public interface IEntitySet<T>
        where T : struct
    {
        EntityEnumerator<T> GetEnumerator();

        IEntitySet<T> Excluding<TExc>() where TExc : struct;
    }

    public interface IEntitySet<T1, T2>
        where T1 : struct
        where T2 : struct
    {
        EntityEnumerator<T1, T2> GetEnumerator();

        IEntitySet<T1, T2> Excluding<TExc>() where TExc : struct;
    }
}