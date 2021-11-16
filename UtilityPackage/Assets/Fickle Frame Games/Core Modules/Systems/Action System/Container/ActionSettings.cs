using UnityEngine;

namespace FickleFrameGames.Systems.Internal
{
    [System.Serializable]
    public class ActionSettings
    {
        public string ActionName;
        public EActionExecutionMode ActionExecutionMode;
        public bool ShouldRegister;
        public float ActionDelay;
        public EOnActionExecutionEnd OnActionExecutionEnd;
        public string NextActionName;
        public ActionComponent NextAction;
        public float DestroyDelay;
#if UNITY_EDITOR
        [SerializeField]
        private int selection;
#endif
    }
}
