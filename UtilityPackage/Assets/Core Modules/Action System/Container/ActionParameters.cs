using UnityEngine;


namespace FickleFrames.Systems
{
    /// <summary>
    /// Container class used to parse data that has to be passed
    /// </summary>
    public class ActionParameters : IActionParameters
    {
        public ActionParameters(object data = null, GameObject source = null)
        {
            Data = data;
            Source = source;
        }

        public object Data { get; set; }
        public GameObject Source { get; set; }
    }
}