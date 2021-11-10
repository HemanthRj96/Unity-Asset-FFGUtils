namespace FickleFrames.Managers.Internal
{
    /// <summary>
    /// Container class to parse SubGameManager data
    /// </summary>
    [System.Serializable]
    public struct SubGameManagerContainer
    {
        public string SubManagerName;
        public SubGameManagerBase SubManager;
    }
}
