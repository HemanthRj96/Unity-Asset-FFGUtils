using System.Collections.Generic;
using UnityEngine;


namespace FFG
{
    public sealed class AnimationController : MonoBehaviour
    {
        #region Editor
#if UNITY_EDITOR
#pragma warning disable 0414
        [SerializeField] private int selection = 0;
#pragma warning restore 0414
#endif
        #endregion Editor


        // Fields

#pragma warning disable 0414
        [SerializeField] private string _animationClipSource = "";
        [SerializeField] private string _animationControllerSavePath = "";
        [SerializeField] private bool _enableAutoUpdate = false;
        [SerializeField] private Animator _animator = null;
#pragma warning restore 0414

        private string _currentState = "";


        // Properties

        private bool validAnimator => _animator != null;


        // Private methods

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
        private void autoAnimationUpdate(List<string> states)
        {
            foreach (string s in states)
                animationPlayer(s);
        }


        // Public methods

        /// <summary>
        /// Method to play an animation
        /// </summary>
        /// <param name="stateName">Target animation state to be played</param>
        public void PlayState(string stateName)
        {
            animationPlayer(stateName);
        }


        /// <summary>
        /// Plays animation if the condition is true
        /// </summary>
        /// <param name="condition">Should be true to play animation</param>
        /// <param name="stateName">Target animation state to be played</param>
        public void PlayStateIf(bool condition, string stateName)
        {
            if (condition)
                animationPlayer(stateName);
        }
    }
}