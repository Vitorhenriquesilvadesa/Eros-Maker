using Descriptor.Grid;
using EventSystem.EventSystem;

namespace Event.Grid
{
    public class ChangeGridActiveLayerEvent : ReactiveEvent
    {
        public readonly GridLayerDescriptor Descriptor;

        public ChangeGridActiveLayerEvent(GridLayerDescriptor descriptor)
        {
            Descriptor = descriptor;
        }
    }
}