

namespace FickleFrames.Systems
{
    public interface IActionSlave
    {
        ActionComponent component { get; }
        void doAction(IActionParameters parameters);
    }
}