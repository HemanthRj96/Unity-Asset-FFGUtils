using UnityEngine;


namespace FickleFrames.Systems
{
    public interface IActionParameters
    {
        object data { get; set; }
        GameObject source { get; set; }
    } 
}