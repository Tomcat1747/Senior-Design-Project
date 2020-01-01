namespace Saving
{
    public interface Saveable
    {
        object CaptureState();

        void RestoreState(object state);
    }
}
