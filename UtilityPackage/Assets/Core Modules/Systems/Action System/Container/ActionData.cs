using UnityEngine;


namespace FickleFrames.Systems
{
    /// <summary>
    /// Container struct used to parse data that has to be passed
    /// </summary>
    public struct ActionData : IActionData
    {
        public ActionData(object data = null, GameObject source = null)
        {
            Data = data;
            Source = source;
        }

        public object Data { get; set; }
        public GameObject Source { get; set; }
    }
}