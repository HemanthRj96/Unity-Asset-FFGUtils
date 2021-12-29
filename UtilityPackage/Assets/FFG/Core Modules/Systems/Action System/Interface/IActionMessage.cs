using UnityEngine;


namespace FFG.Systems
{
    public interface IActionMessage
    {
        object Data { get; set; }
        GameObject Source { get; set; }
    } 
}