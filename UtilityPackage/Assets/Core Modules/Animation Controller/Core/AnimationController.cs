using UnityEngine;


namespace FickleFrames.Controllers
{
    public class AnimationController : MonoBehaviour
    {
        #region Editor
#if UNITY_EDITOR
        [SerializeField] private int selection = 0;
#endif
        #endregion Editor

        #region Internal

        /*.............................................Serialized Fields....................................................*/
#pragma warning disable 0649, 0414

        [SerializeField] private string _animationClipSource = "";
        [SerializeField] private string _animationControllerSavePath = "";
        [SerializeField] private bool _enableAutoUpdate = false;
        [SerializeField] private Animator _animator;
        [SerializeField] private StateController _stateController;


        /*.............................................Private Fields.......................................................*/
        private string _currentState;

#pragma warning restore 0649, 0414


        /*.............................................Properties...........................................................*/
        private bool validAnimator => _animator != null;

        #region Private Methods

        /*.............................................Private Methods......................................................*/
        /// <summary>
        /// Bootstraps on start
        /// </summary>
        private void Awake()
        {
            bootStrapper();
        }


        /// <summary>
        /// Initializes values and data
        /// </summary>
        private void bootStrapper()
        {
            if (_stateController == null)
                return;

            // Attach based on animation update
            if (_enableAutoUpdate)
                _stateController.AttachStateChangeEvent(autoAnimationUpdate);
            else
                _stateController.AttachStateChangeEvent(manualAnimationUpdate);
        }


        /// <summary>
        /// Method to play an animation
        /// </summary>
        private void animationPlayer(string state)
        {
            if (validAnimator == false || _currentState == state)
                return;

            _animator.Play(state);
            _currentState = state;
        }

        /// <summary>
        /// This function is automatically invoked if automatic animation update is set to true
        /// </summary>
        private void autoAnimationUpdate(string newState)
        {
            animationPlayer(newState);
        }

        #endregion Private Methods

        #endregion Internal

        #region Protected Methods

        /*.............................................Protected Methods....................................................*/
        /// <summary>
        /// Override this method to extend functionality
        /// </summary>
        protected virtual void manualAnimationUpdate(string stateName) { }

        #endregion Protected Methods

        #region Public Methods

        /*.............................................Public Methods.......................................................*/
        /// <summary>
        /// Method to play an animation
        /// </summary>
        /// <param name="state">Target animation state to be played</param>
        public virtual void PlayState(string state)
        {
            animationPlayer(state);
        }


        /// <summary>
        /// Plays animation if the condition is true
        /// </summary>
        /// <param name="condition">Should be true to play animation</param>
        /// <param name="state">Target animation state to be played</param>
        public virtual void PlayStateIf(bool condition, string state)
        {
            if (condition)
                animationPlayer(state);
        }

        #endregion Public Methods
    }
}