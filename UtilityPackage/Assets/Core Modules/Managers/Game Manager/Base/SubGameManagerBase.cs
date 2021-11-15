using UnityEngine;

namespace FickleFrames.Managers
{
    public abstract class SubGameManagerBase : MonoBehaviour
    {
        /*.............................................Private Fields.......................................................*/

        private bool _started = false;
        private ESubManagerState _state = ESubManagerState.Running;

        /*.............................................Protected Methods....................................................*/
        
        /// <summary>
        /// This method will only run once in the first call and wouldn't run again
        /// </summary>
        protected abstract void OnBegin();


        /// <summary>
        /// This method will run all the update loop and returns if the loop is done or not. When the loop is done the OnStop method is called
        /// which would return a string that corresponds to the next SubManager
        /// </summary>
        protected abstract ESubManagerState OnUpdate();


        /// <summary>
        /// Runs only once when the update logic is done. It returns a string tag that corresponds to the tag given to SubManager. 
        /// If this SubManager logic has to be run again then return default as the parameter
        /// </summary>
        protected abstract string OnEnd();

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Call this method to run a sub manager routine
        /// </summary>
        public string Run()
        {
            if (_started == false)
            {
                _started = true;
                OnBegin();
            }

            _state = OnUpdate();

            if (_state == ESubManagerState.Done)
            {
                _started = false;
                return OnEnd();
            }
            else
                return default;
        }
    }
}