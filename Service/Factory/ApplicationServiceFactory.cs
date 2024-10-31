using Service.Application;

namespace Service.Factory
{
    public abstract class ApplicationServiceFactory
    {
        public abstract ApplicationService Get<S>() where S : ApplicationService;
    }
}