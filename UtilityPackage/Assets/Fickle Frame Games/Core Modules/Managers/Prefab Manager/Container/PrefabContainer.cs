using UnityEngine;

/// <summary>
/// Container used by PrefabManager to store information about prefab
/// </summary>
namespace FickleFrameGames.Managers.Internal
{
    [System.Serializable]
    public class PrefabContainer
    {
        public string PrefabName;
        public GameObject Prefab;
    }
}