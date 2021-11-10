using UnityEngine;


namespace FickleFrames.Systems
{
    public interface IActionData
    {
        object Data { get; set; }
        GameObject Source { get; set; }
    } 
}