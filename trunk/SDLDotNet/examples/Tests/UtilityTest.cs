using NUnit.Framework;
using System.Drawing;

namespace MfGames.Utility
{
  [TestFixture] public class UtilityTest
  {
    [Test] public void TestDefaultsVector2()
    {
      Vector2 v = new Vector2();
      Assert.Equals(0, v.X);
      Assert.Equals(0, v.Y);
    }

    [Test] public void TestDefaultsVector3()
    {
      Vector3 v = new Vector3();
      Assert.Equals(0, v.X);
      Assert.Equals(0, v.Y);
      Assert.Equals(0, v.Z);
    }
  }
}
