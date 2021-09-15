using System.Collections.Generic;
using UnityEngine;

namespace FickleFrames.StateMachine
{
    /// <summary>
    /// This class does not require any kind of modifications
    /// </summary>
    [CreateAssetMenu(fileName = "New StateSettingsContainer", menuName = "Scriptable Objects/StateSettingsContainer")]
    public class StateSettingsContainer : ScriptableObject
    {
        public SettingsContainer settings;
    }
}