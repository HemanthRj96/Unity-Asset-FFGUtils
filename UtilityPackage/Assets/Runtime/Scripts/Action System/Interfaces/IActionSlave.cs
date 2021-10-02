using FickleFrames;

public interface IActionSlave
{
    ActionComponent component { set; }
    void doAction(IActionParameters parameters);
}
