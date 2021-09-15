//##IF NEW STATE SETTINGS ARE TO BE ADDED THEN ADD BELOW
namespace FickleFrames.StateMachine
{
    /// <summary>
    /// This class is a collection of settings for all the different states
    /// </summary>
    [System.Serializable]
    public class SettingsContainer
    {
        //##NEW STATE SETTING SHOULD BE ADDED HERE##
        public IdleSettings idleSettings;
        public RunSettings runSettings;
        public WalkSettings walkSettings;
        public JumpSettings jumpSettings;
        public DashSettings dashSettings;
        public OnAirSettings onAirSettings;
        public GlideSettings glideSettings;
        //##NEW STATE SETTING SHOULD BE ADDED HERE##
    }

    /// <summary>
    /// This is a base class for state settings, so if you create a new state inherit from this class 
    /// and add it below
    /// </summary>
    public class StateSettings { }

    //##ALL CUSTOM STATE SETTINGS SHOULD BE ADDED HERE##

    /// <summary>
    /// This class contains all the settings for idle
    /// </summary>
    [System.Serializable]
    public class IdleSettings : StateSettings
    {

    }

    /// <summary>
    /// This class contains all the settings for running
    /// </summary>
    [System.Serializable]
    public class RunSettings : StateSettings
    {
        public float runSpeed;
    }

    /// <summary>
    /// This class contains all the settings for jumping
    /// </summary>
    [System.Serializable]
    public class JumpSettings : StateSettings
    {
        public float jumpHeight;
    }

    /// <summary>
    /// This class contains all the settings for walking
    /// </summary>
    [System.Serializable]
    public class WalkSettings : StateSettings
    {
        public float walkSpeed;
    }

    /// <summary>
    /// This class contains all the settings for dashing 
    /// </summary>
    [System.Serializable]
    public class DashSettings : StateSettings
    {
        public float dashDistance;
        public float dashingSpeed;
    }

    /// <summary>
    /// This class contains all the settings for gliding
    /// </summary>
    [System.Serializable]
    public class GlideSettings : StateSettings
    {
        public float glideSpeed;
    }

    /// <summary>
    /// This class contains all the settings for on air
    /// </summary>
    [System.Serializable]
    public class OnAirSettings : StateSettings
    {
        public float airControl;
    }

    //##ALL CUSTOM STATE SETTINGS SHOULD BE ADDED HERE##
}