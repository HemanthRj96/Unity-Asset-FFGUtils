namespace FickleFrames.ActionSystem
{
    public interface IActionSlave
    {
        ActionComponent usingComponent { set; }
        void doAction(IActionParameters parameters);
    }
}