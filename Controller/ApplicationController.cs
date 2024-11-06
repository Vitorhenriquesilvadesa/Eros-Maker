using EventSystem;
using UnityEngine;

namespace Controller
{
    public abstract class ApplicationController<T> : MonoBehaviour where T : ApplicationController<T>
    {
        protected ApplicationController()
        {
            EventAPI.Subscribe(this);
        }

        ~ApplicationController()
        {
            EventAPI.Unsubscribe(this);
        }
    }
}