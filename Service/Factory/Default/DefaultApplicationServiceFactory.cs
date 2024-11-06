using Service.Application;
using Service.Application.Exception;

namespace Service.Factory.Default
{
    public class DefaultApplicationServiceFactory : ApplicationServiceFactory
    {
        public override ApplicationService Get<S>()
        {
            return typeof(S) switch
            {
                { } t when t == typeof(GridManagementService) => new GridManagementService(),
                _ => throw new UnknownServiceException()
            };
        }
    }
}