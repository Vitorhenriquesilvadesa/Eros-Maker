using ErosEditor.Service.Scripting;
using Service.Application.Exception;
using Service.Initialization;
using Service.Initialization.Grid;

namespace Service.Factory.Default
{
    public class DefaultInitializationServiceFactory : InitializationServiceFactory
    {
        public override InitializationService Get<S>()
        {
            switch (typeof(S))
            {
                case { } t when t == typeof(GridConfigurationService):
                    return new GridConfigurationService();

                default:
                    throw new UnknownServiceException();
            }
        }
    }
}