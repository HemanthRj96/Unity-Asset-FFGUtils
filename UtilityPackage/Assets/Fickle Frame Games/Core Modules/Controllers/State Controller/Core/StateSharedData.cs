using UnityEngine;

namespace FickleFrameGames.Controllers
{
    public class StateSharedData : ScriptableObject
    {
        /*.............................................Private Fields.......................................................*/

        private StateController _parentController = null;
        
        /*.............................................Properties...........................................................*/

        public StateController ParentController
        {
            get { return _parentController; }
            set { if (_parentController == null) _parentController = value; }
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
    }
}