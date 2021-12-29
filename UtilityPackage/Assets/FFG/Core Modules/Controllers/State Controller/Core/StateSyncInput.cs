using UnityEngine;

namespace FFG.Controllers
{
    public abstract class StateSyncInput : ScriptableObject
    {
        /*.............................................Private Fields.......................................................*/

        private StateController _parentController = null;
        private StateSharedData _sharedData = null;

        /*.............................................Properties...........................................................*/

        public StateController ParentController
        {
            get { return _parentController; }
            set { if (_parentController == null) _parentController = value; }
        }

        public StateSharedData SharedData
        {
            get { return _sharedData; }
            set { if (_sharedData == null) _sharedData = value; }
        }

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Override this method if any initialization or any other operations has to be performed on Awake
        /// (Note: this method is invoked before all states)
        /// </summary>
        public virtual void StateAwake() { }


        /// <summary>
        /// Override this method if any initialization or any other operations has to be performed on Start
        /// (Note: this method is invoked before all states)
        /// </summary>
        public virtual void StateStart() { }


        /// <summary>
        /// Returns the corresponding name of the state upon updating the input
        /// </summary>
        public abstract string InputUpdate();
    }
}