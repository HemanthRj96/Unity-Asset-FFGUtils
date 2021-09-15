namespace FickleFrames.StateMachine
{
    /// <summary>
    /// This is the base class of custom states, if you have to add new custom state then you should 
    /// inherit from this class and update StateController and SettingsContainer
    /// </summary>
    public class State : I_State
    {
        public State(StateSettings settings)
        {
            this.settings = settings;
        }

        protected StateSettings settings;

        /// <summary>
        /// Override this method
        /// </summary>
        virtual public I_State getState() { return this; }

        /// <summary>
        /// Override this method
        /// </summary>
        virtual public void updateState() { }
    }
}