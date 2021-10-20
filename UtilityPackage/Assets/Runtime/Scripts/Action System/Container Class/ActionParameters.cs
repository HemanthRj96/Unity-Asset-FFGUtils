using UnityEngine;


namespace FickleFrames.ActionSystem
{
    /// <summary>
    /// Container class used to parse data that has to be passed
    /// </summary>
    public class ActionParameters : IActionParameters
    {
        #region Public Constructors

        public ActionParameters(object data = null, GameObject source = null)
        {
            this.data = data;
            this.source = source;
        }

        #endregion Public Constructors

        #region Public Properties

        public object data { get; set; }
        public GameObject source { get; set; }

        #endregion Public Properties
    }
}