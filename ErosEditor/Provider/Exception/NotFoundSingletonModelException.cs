using System.Reflection;

namespace Provider.Exception
{
    public class NotFoundSingletonModelException : System.Exception
    {
        public NotFoundSingletonModelException(MemberInfo type) : base($"No get source provided to {type.Name}.")
        {
            
        }
    }
}