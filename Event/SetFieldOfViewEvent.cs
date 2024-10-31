using EventSystem.EventSystem;

namespace Event
{
    public class SetFieldOfViewEvent : ReactiveEvent
    {
        public readonly float FOV;

        public SetFieldOfViewEvent(float fov)
        {
            FOV = fov;
        }
    }
}