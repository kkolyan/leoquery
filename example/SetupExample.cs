namespace Kk.LeoQuery
{
    public class SetupExample
    {
        private ISystem _pipeline;
        private IEntityStorage _storage;

        private void Start()
        {
            _storage = new LeoLiteStorage();
            
            Injector di = new Injector()
                .Add(new Service1());
            
            _pipeline = new MulticastSystem()
                .Add(new System1())
                .ForEach(di.InjectInto);
        }

        private void Update()
        {
            _pipeline.Act(_storage);
        }
    }
}