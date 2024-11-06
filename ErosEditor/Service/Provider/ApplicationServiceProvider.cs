using ErosEditor.Service.Factory.Default;
using Service.Application;
using Service.Factory;
using Service.Factory.Default;

namespace Service.Provider
{
    public class ApplicationServiceProvider : ServiceProvider<ApplicationService>
    {
        private readonly ApplicationServiceFactory _factory = new DefaultApplicationServiceFactory();

        public override ApplicationService Get<S>()
        {
            return _factory.Get<S>();
        }
    }
}