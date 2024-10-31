using Service.Factory;
using Service.Factory.Default;
using Service.Initialization;

namespace Service.Provider
{
    public abstract class ServiceProvider<T>
    {
        private InitializationServiceFactory _serviceFactory = new DefaultInitializationServiceFactory();
        public abstract T Get<S>() where S : T;
    }
}