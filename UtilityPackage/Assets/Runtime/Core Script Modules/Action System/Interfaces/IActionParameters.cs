using UnityEngine;


namespace FickleFrames.Systems
{
    public interface IActionParameters
    {
        object Data { get; set; }
        GameObject Source { get; set; }
    } 
}