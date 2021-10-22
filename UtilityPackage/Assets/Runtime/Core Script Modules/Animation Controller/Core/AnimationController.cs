using UnityEngine;


namespace FickleFrames.Controllers
{
    /// <summary>
    /// Inherit this class if manual animation update is required
    /// </summary>
    public class AnimationController : MonoBehaviour
    {
        #region Editor
#if UNITY_EDITOR
#pragma warning disable 0649, 0414
        [SerializeField] private int selection = 0;
#pragma warning restore 0649, 0414
#endif
#endregion Editor

        #region Private Fields
#pragma warning disable 0649, 0414

        [SerializeField] private string animationClipSource = "";
        [SerializeField] private string animationControllerSavePath = "";
        [SerializeField] private bool enableAutoUpdate = false;
        [SerializeField] private Animator animator;
        [SerializeField] private StateController stateController;
        private string currentState;

#pragma warning restore 0649, 0414
        #endregion Private Fields

        #region Private Properties

        private bool validAnimator => animator != null;

        #endregion Private Properties

        #region Private Methods

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
            if (stateController == null)
                return;

            // Attach based on animation update
            if (enableAutoUpdate)
                stateController.AttachStateChangeEvent(autoAnimationUpdate);
            else
                stateController.AttachStateChangeEvent(manualAnimationUpdate);
        }


        /// <summary>
        /// Method to play an animation
        /// </summary>
        private void animationPlayer(string state)
        {
            if (validAnimator == false || currentState == state)
                return;

            animator.Play(state);
            currentState = state;
        }


        /// <summary>
        /// This function is automatically invoked if automatic animation update is set to true
        /// </summary>
        private void autoAnimationUpdate(string newState)
        {
            animationPlayer(newState);
        }

        #endregion Private Methods

        #region Protected Methods

        /// <summary>
        /// Override this method to extend functionality
        /// </summary>
        protected virtual void manualAnimationUpdate(string stateName) { }

        #endregion Protected Methods

        #region Public Methods

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