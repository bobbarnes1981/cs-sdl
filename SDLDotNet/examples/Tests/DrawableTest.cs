using NUnit.Framework;

namespace MfGames.Sdl.Drawable
{
  [TestFixture] public class DrawableTest
  {
    [Test] public void TestImageLoad()
    {
      // Load the image
      ImageDrawable id = new ImageDrawable("../../Data/marble1.png");
      
      // Make sure the height and width match
      Assert.Equals(384, id.Size.Width);
      Assert.Equals(384, id.Size.Height);
    }
  }
}
