using ErosEditor.Service.Scripting;
using Service.Application;
using Service.Application.Exception;
using Service.Factory;

namespace ErosEditor.Service.Factory.Default
{
    public class DefaultApplicationServiceFactory : ApplicationServiceFactory
    {
        public override ApplicationService Get<S>()
        {
            return typeof(S) switch
            {
                { } t when t == typeof(GridManagementService) => new GridManagementService(),
                { } t when t == typeof(ScriptingEngineService) => new ScriptingEngineService(),
                _ => throw new UnknownServiceException()
            };
        }
    }
}