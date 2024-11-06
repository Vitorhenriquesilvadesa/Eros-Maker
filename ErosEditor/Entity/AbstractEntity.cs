using Descriptors;

namespace Entity
{
    public abstract class AbstractEntity<T> where T : AbstractEntity<T>
    {
        public abstract AbstractDescriptor<T> GetDescriptor();
        public abstract void Destroy();
    }
}