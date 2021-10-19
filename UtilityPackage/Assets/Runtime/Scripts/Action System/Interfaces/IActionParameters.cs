using UnityEngine;


namespace FickleFrames.ActionSystem
{
    public interface IActionParameters
    {
        object data { get; set; }
        GameObject source { get; set; }
    } 
}