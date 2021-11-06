using FickleFrames.Managers.Internal;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace FickleFrames.Managers
{
    /// <summary>
    /// Singleton class which controls all the game state through it's sub game managers
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        #region Internal

        /*.............................................Serialized Fields....................................................*/
#pragma warning disable 0649
        [SerializeField] private SubGameManagerContainer[] _subManagers;

        /*.............................................Private Fields.......................................................*/
        private string _currentSubManagerName;
        private BaseGameManager _currentSubManager;
        private Dictionary<string, BaseGameManager> _subManagerCollection = new Dictionary<string, BaseGameManager>();
#pragma warning restore 0649


        #region Private Methods

        /*.............................................Private Methods......................................................*/
        private new void Awake() => bootstrap();

        private void Update() => gameManagerUpdate();

        private void bootstrap()
        {
            base.Awake();

            foreach (var manager in _subManagers)
                if (manager.SubManager != null)
                {
                    var instance = Instantiate(manager.SubManager, transform);
                    instance.name = string.IsNullOrEmpty(manager.SubManagerName) ? manager.SubManager.name : manager.SubManagerName;
                    DontDestroyOnLoad(instance);
                    _subManagerCollection.TryAdd(instance.name, instance);
                }
            // Initialize the first memeber inside the collection
            _currentSubManager = _subManagerCollection.First().Value;
        }

        private void gameManagerUpdate()
        {
            _currentSubManagerName = _currentSubManager.Run();

            if (_currentSubManagerName == default)
                return;
            else if (_subManagerCollection.ContainsKey(_currentSubManagerName))
                _currentSubManager = _subManagerCollection[_currentSubManagerName];
            else
            {
                //Handle game manager error
                Debug.LogError("Mismatched Sub Manager Name => " + _currentSubManagerName);
            }
        }

        #endregion Private Methods

        #endregion Internal
    }
}