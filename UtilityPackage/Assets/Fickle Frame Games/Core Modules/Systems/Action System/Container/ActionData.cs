using UnityEngine;


namespace FickleFrameGames.Systems
{
    /// <summary>
    /// Container struct used to parse data that has to be passed
    /// </summary>
    public struct ActionData : IActionMessage
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