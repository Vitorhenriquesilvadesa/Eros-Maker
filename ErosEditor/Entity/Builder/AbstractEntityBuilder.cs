namespace Entity.Builder
{
    public abstract class AbstractEntityBuilder<T> where T : AbstractEntity<T>
    {
        protected T instance;
        public abstract T Build();
    }
}