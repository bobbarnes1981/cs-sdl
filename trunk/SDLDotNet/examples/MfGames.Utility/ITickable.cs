namespace MfGames.Utility
{
  public interface ITickable
  {
    bool IsTickable { get; }
    void OnTick(TickArgs args);
  }
}
