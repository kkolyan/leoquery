namespace Kk.LeoQuery
{
    public interface IRelationsBuilder
    {
        /// <summary>
        /// Satellite components are automatically added and deleted with addition and deletion of owner component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Satellite<T>() where T : struct;
    }
}