namespace FickleFrames.Systems
{
    public interface IActionSlave
    {
        ActionComponent Component { get; }
        void DoAction(IActionParameters parameters);
    }
}