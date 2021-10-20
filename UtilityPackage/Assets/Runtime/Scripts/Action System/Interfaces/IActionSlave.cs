

namespace FickleFrames.ActionSystem
{
    public interface IActionSlave
    {
        ActionComponent component { get; }
        void doAction(IActionParameters parameters);
    }
}