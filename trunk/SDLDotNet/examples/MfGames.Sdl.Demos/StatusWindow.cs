using MfGames.Sdl.Drawable;
using MfGames.Sdl.Gui;
using MfGames.Sdl.Sprites;
using MfGames.Utility;
using SdlDotNet;
using System;
using System.Drawing;

namespace MfGames.Sdl.Demos
{
  /// <summary>
  /// This is a status window which shows the current state of the
  /// system at any time. This allows the system to report anything
  /// required.
  /// </summary>
  public class StatusWindow : GuiWindow
  {
    /// <summary>
    /// Creates a basic status window above everything else.
    /// </summary>
    public StatusWindow(GuiManager manager)
      : base(manager, new Rectangle2(625, 475, 150, 100))
    {
      // Set up our title
      Coords.Z = 2000;
      Title = "Demo Status";
      IsDragable = true;

      // Add some text
      int labelOffset = 2;
      int dataOffset = 54;
      int labelHeight = manager.GetTitleHeight("test");
      int labelPad = 2;
      int labelWidth = 48;
      int dataWidth = 96;
      int i = 0;

      // Add the ticks per second
      Contents.Add(new BoundedTextSprite("TPS:", manager.TitleFont,
					   new Dimension2(labelWidth, labelHeight),
					   1.0, 0.5,
					   new Vector2(labelOffset,
						     (labelHeight
						      + labelPad) * i + 2)));
      tps = new BoundedTextSprite("---", manager.BaseFont,
				  new Dimension2(dataWidth, labelHeight),
				  0.0, 0.5,
				  new Vector2(dataOffset,
					    (labelHeight + labelPad) * i + 2));
      Contents.Add(tps);

      // Add the frames per second
      i++;
      Contents.Add(new BoundedTextSprite("FPS:", manager.TitleFont,
					   new Dimension2(labelWidth, labelHeight),
					   1.0, 0.5,
					   new Vector2(labelOffset,
						     (labelHeight
						      + labelPad) * i + 2)));
      fps = new BoundedTextSprite("---", manager.BaseFont,
				  new Dimension2(dataWidth, labelHeight),
				  0.0, 0.5,
				  new Vector2(dataOffset,
					    (labelHeight + labelPad) * i + 2));
      Contents.Add(fps);

      // Add the current mode
      i++;
      Contents.Add(new BoundedTextSprite("Mode:", manager.TitleFont,
					   new Dimension2(labelWidth, labelHeight),
					   1.0, 0.5,
					   new Vector2(labelOffset,
						     (labelHeight
						      + labelPad) * i + 2)));
      mode = new BoundedTextSprite("---", manager.BaseFont,
				   new Dimension2(dataWidth, labelHeight),
				   0.0, 0.5,
				   new Vector2(dataOffset,
					     (labelHeight + labelPad)
					     * i + 2));
      Contents.Add(mode);

      // Add the instructions
      i++;
      Contents.Add(new BoundedTextSprite("Press the number keys",
					   manager.BaseFont,
					   new Dimension2(150, labelHeight),
					   0.5, 0.5,
					   new Vector2(labelOffset,
						     (labelHeight
						      + labelPad) * i + 2)));

      // Add ourselves to the ticker
      SdlDemo.TickManager.Add(this);

      // Adjust our height
      i++;
      Size.Height = (labelHeight + labelPad) * i + 4;
    }

    #region Data Components
    private BoundedTextSprite tps = null;
    private BoundedTextSprite fps = null;
    private BoundedTextSprite mode = null;
    #endregion

    #region Animation
    public override void OnTick(TickArgs args)
    {
      tps.Text = String.Format("{0}", SdlDemo.TickManager.TicksPerSecond);

      if (SdlDemo.Fps.IsFull)
	fps.Text = SdlDemo.Fps.Average.ToString("#0.00");
      else
	fps.Text = "---";

      if (SdlDemo.CurrentDemo == null)
	mode.Text = "<none>";
      else
	mode.Text = SdlDemo.CurrentDemo.ToString();
    }
    #endregion
  }
}
