using UnityEngine;


namespace FickleFrameGames.Systems
{
    public interface IActionMessage
    {
        object Data { get; set; }
        GameObject Source { get; set; }
    } 
}