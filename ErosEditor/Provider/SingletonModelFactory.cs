using Model;
using Provider.Exception;

namespace Provider
{
    public class SingletonModelFactory
    {
        public AbstractModel Create<T>() where T : AbstractModel
        {
            return typeof(T) switch
            {
                { } t when t == typeof(Player) => new Player(),
                { } t when t == typeof(CameraConfiguration) => new CameraConfiguration(),
                _ => throw new NotFoundSingletonModelException(typeof(T))
            };
        }
    }
}