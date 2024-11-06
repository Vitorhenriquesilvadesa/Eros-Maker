using Service.Initialization;

namespace Service.Factory
{
    public abstract class InitializationServiceFactory
    {
        public abstract InitializationService Get<S>();
    }
}