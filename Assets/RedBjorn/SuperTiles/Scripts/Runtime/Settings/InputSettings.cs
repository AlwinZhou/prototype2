using RedBjorn.Utils;
using System;
using UnityEngine;

namespace RedBjorn.SuperTiles.Settings
{
    /// <summary>
    /// Settings which contains data about keycodes which is paired to corresponding input actions
    /// </summary>
    public class InputSettings : ScriptableObjectExtended
    {
        [Serializable]
        public struct Key 
        {
            public KeyCode Main;
            public KeyCode Alternative;
        }

        public Key WorldClick;
        public Key TurnComplete;
        public Key CameraClockwise;
        public Key CameraCounterClockwise;
        [Space]
        public Key Menu;
        public Key DebugUi;
        [Space]
        public Key Submit;
        public Key Cancel;
    }
}
