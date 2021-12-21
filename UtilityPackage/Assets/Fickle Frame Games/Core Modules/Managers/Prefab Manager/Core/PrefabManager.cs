using FickleFrameGames.Managers.Internal;
using System.Linq;
using UnityEngine;


namespace FickleFrameGames.Managers
{
    [CreateAssetMenu(menuName = "Fickle Frame Games/Create New PrefabManager [Type: ScriptableObject, FileType: Asset]")]
    public class PrefabManager : ScriptableObject
    {
        /*.............................................Serialized Fields....................................................*/

        [SerializeField]
        private PrefabContainer[] _prefabContainer;

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Returns a prefab
        /// </summary>
        /// <param name="prefabName">Name of the prefab</param>
        public GameObject GetPrefab(string prefabName)
        {
            return _prefabContainer.ToList().Find(x => x.PrefabName == prefabName).Prefab;
        }
    }
}