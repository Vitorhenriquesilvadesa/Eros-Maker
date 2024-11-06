using Service.Factory;
using Service.Factory.Default;
using Service.Initialization;

namespace Service.Provider
{
    public class InitializationServiceProvider : ServiceProvider<InitializationService>
    {
        private readonly InitializationServiceFactory _factory = new DefaultInitializationServiceFactory();

        public override InitializationService Get<S>()
        {
            return _factory.Get<S>();
        }
    }
}