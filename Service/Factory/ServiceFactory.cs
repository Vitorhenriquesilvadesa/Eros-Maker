using Service.Factory.Default;
using Service.Initialization;

namespace Service.Factory
{
    public abstract class ServiceFactory<T>
    {
        public abstract T Get<S>() where S : T;
    }
}