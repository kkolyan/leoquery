using Kk.LeoQuery;

namespace leoquery.test
{
    public class System1 : ISystem
    {
        [Inject] private Service1 _service1;

        public void Act(IEntityStorage storage)
        {
            foreach (Entity<Component1> entity in storage.Query<Component1>())
            {
                if (entity.Has<Component2>())
                {
                    entity.Get<Component2>().value = entity.Get1().value * 2;
                }
                else
                {
                    storage.NewEntity()
                        .Add(new Component2
                        {
                            value = entity.Get1().value * 2
                        });
                }

                if (entity.Has<Component3>())
                {
                    entity.Del<Component3>();
                }
                else
                {
                    entity.Destroy();
                }
                
            }
        }
    }
}