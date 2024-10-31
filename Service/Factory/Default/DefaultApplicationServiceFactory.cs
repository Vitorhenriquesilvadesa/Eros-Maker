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
                { } t when t == typeof(PrintHelloService) => new PrintHelloService(),
                { } t when t == typeof(WhoSayHelloService) => new WhoSayHelloService(),
                _ => throw new UnknownServiceException()
            };
        }
    }
}