using System;

namespace MfGames.Utility
{
  /// <summary>
  /// A simple guage which attempts to keep track of how many ticks or
  /// activations per second. This is used to calculate the FPS of a
  /// program.
  /// </summary>
  public class SecondGuage : ITickable
  {
    private int span = 5;

    private uint [] buckets = null;

    public SecondGuage()
    {
      buckets = new uint [span];

      for (int i = 0; i < span; i++)
	buckets[i] = 0;
    }

    public SecondGuage(int newSpan)
    {
      span = newSpan;
      buckets = new uint [span];

      for (int i = 0; i < span; i++)
	buckets[i] = 0;
    }

    #region Counting
    private long lastSecond = DateTime.Now.Second;

    private long loops = 0;

    public bool IsTickable { get { return true; } }

    /// <summary>
    /// Activates the counter for the current second.
    /// </summary>
    public void Activate()
    {
      lock (this) {
	// Figure if we need a roll-over
	long now = DateTime.Now.Second;

	if (now != lastSecond)
	{
	  // Set up the new time
	  lastSecond = now;
	  loops++;

	  // Move the buckets over
	  for (int i = 1; i < span; i++)
	  {
	    buckets[i] = buckets[i - 1];
	  }

	  // Reset the last one
	  buckets[0] = 0;
	}
	
	// Add to the bucket
	buckets[0]++;
      }
    }

    /// <summary>
    /// Enables the guage to be added directly to a tick manager.
    /// </summary>
    public void OnTick(TickArgs args)
    {
      Activate();
    }
    #endregion

    #region Results
    public double Average
    {
      get
      {
	double total = 0.0;

	for (int i = 1; i < span; i++)
	  total += buckets[i];

	return total / span;
      }
    }

    public bool IsFull { get { return loops >= span; } }
    #endregion
  }
}
