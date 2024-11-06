using Mapping;
using UnityEngine;

namespace Controller
{
    public sealed class InputController
    {
        private static readonly KeyboardActionMapping KeyboardActionMapping = new();
        
        public static bool GetAction(KeyboardAction action)
        {
            return KeyboardActionMapping.GetAction(action);
        }

        public static bool GetTrigger(KeyboardAction action)
        {
            return KeyboardActionMapping.GetTrigger(action);
        }

        public static bool GetTriggerRelease(KeyboardAction action)
        {
            return KeyboardActionMapping.GetTriggerRelease(action);
        }

        public static float GetAxis(string axis)
        {
            return Input.GetAxis(axis);
        }
    }
}