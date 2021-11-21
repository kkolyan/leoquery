namespace Kk.LeoQuery
{
    public interface IComponentGroupBuilder
    {
        void AddMember<T>() where T : struct;
    }
}