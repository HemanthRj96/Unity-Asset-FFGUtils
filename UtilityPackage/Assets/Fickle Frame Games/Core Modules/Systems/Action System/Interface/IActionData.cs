using UnityEngine;


namespace FickleFrameGames.Systems
{
    public interface IActionData
    {
        object Data { get; set; }
        GameObject Source { get; set; }
    } 
}