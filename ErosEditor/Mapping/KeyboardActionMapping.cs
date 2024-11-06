using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mapping
{
    public class KeyboardActionMapping
    {
        private Dictionary<KeyboardAction, Func<bool>> actionMap = new();
        private Dictionary<KeyboardAction, Func<bool>> triggerMap = new();
        private Dictionary<KeyboardAction, Func<bool>> triggerReleaseMap = new();

        public KeyboardActionMapping()
        {
            actionMap.Add(KeyboardAction.Up, () => Input.GetKey(KeyCode.W));
            actionMap.Add(KeyboardAction.Down, () => Input.GetKey(KeyCode.S));
            actionMap.Add(KeyboardAction.Left, () => Input.GetKey(KeyCode.A));
            actionMap.Add(KeyboardAction.Right, () => Input.GetKey(KeyCode.D));
            actionMap.Add(KeyboardAction.Interact, () => Input.GetKey(KeyCode.Space));
            actionMap.Add(KeyboardAction.Run, () => Input.GetKey(KeyCode.LeftShift));
            triggerMap.Add(KeyboardAction.Up, () => Input.GetKeyDown(KeyCode.W));
            triggerMap.Add(KeyboardAction.Down, () => Input.GetKeyDown(KeyCode.S));
            triggerMap.Add(KeyboardAction.Left, () => Input.GetKeyDown(KeyCode.A));
            triggerMap.Add(KeyboardAction.Right, () => Input.GetKeyDown(KeyCode.D));
            triggerMap.Add(KeyboardAction.Interact, () => Input.GetKeyDown(KeyCode.Space));
            triggerMap.Add(KeyboardAction.Run, () => Input.GetKeyDown(KeyCode.LeftShift));
            triggerReleaseMap.Add(KeyboardAction.Up, () => Input.GetKeyUp(KeyCode.W));
            triggerReleaseMap.Add(KeyboardAction.Down, () => Input.GetKeyUp(KeyCode.S));
            triggerReleaseMap.Add(KeyboardAction.Left, () => Input.GetKeyUp(KeyCode.A));
            triggerReleaseMap.Add(KeyboardAction.Right, () => Input.GetKeyUp(KeyCode.D));
            triggerReleaseMap.Add(KeyboardAction.Interact, () => Input.GetKeyUp(KeyCode.Space));
            triggerReleaseMap.Add(KeyboardAction.Run, () => Input.GetKeyUp(KeyCode.LeftShift));
        }

        public bool GetAction(KeyboardAction keyboardAction)
        {
            return actionMap[keyboardAction]();
        }

        public bool GetTrigger(KeyboardAction keyboardAction)
        {
            return triggerMap[keyboardAction]();
        }

        public bool GetTriggerRelease(KeyboardAction keyboardAction)
        {
            return triggerReleaseMap[keyboardAction]();
        }
    }
}