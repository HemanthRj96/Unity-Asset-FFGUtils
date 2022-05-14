namespace FFG.Systems.Internal
{
    public interface IPoolSlave
    {
        void OnUse();
        void OnRelease();
    } 
}