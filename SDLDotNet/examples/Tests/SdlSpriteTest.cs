using NUnit.Framework;
using System.Drawing;

namespace MfGames.Sdl.Sprites
{
  [TestFixture] public class SdlSpriteTest
  {
    /*
    [Test] public void TestEmptyManager()
    {
      SpriteManager sm = new SpriteManager();
      Assertion.AssertEquals(0, sm.GetSprites().Count);
    }

    [Test] public void TestSingleSprite()
    {
      SpriteManager sm = new SpriteManager();
      Sprite s = new Sprite();
      sm.Add(s);
      Assertion.AssertEquals(1, sm.GetSprites().Count);
    }
    
    [Test] public void TestSpriteAddedTwice()
    {
      SpriteManager sm = new SpriteManager();
      Sprite s = new Sprite();
      
      sm.Add(s);
      sm.Add(s);
      Assertion.AssertEquals(1, sm.GetSprites().Count);
    }
    
    [Test] public void TestSpriteRemoved()
    {
      SpriteManager sm = new SpriteManager();
      Sprite s = new Sprite();
      
      sm.Add(s);
      sm.Remove(s);
      Assertion.AssertEquals(0, sm.GetSprites().Count);
    }

    [Test] public void TestNestedSpriteManager()
    {
      SpriteManager sm = new SpriteManager();
      SpriteManager sm1 = new SpriteManager();
      Sprite s = new Sprite();
      
      sm1.Add(s);
      sm.Add(sm1);
      Assertion.AssertEquals(1, sm.GetSprites().Count);
    }

    [Test] public void TestTranslatedSpriteZ()
    {
      Sprite s = new Sprite(new Point(10, 13), 6);
      Sprite t = new TranslateSprite(s, 5, 8);

      Assertion.AssertEquals(s.Z, t.Z);
    }

    [Test] public void TestTranslatedSpriteX()
    {
      Sprite s = new Sprite(new Point(10, 13), 6);
      Sprite t = new TranslateSprite(s, 5, 8);

      Assertion.AssertEquals(s.X + 5, t.X);
    }

    [Test] public void TestTranslatedSpriteY()
    {
      Sprite s = new Sprite(new Point(10, 13), 6);
      Sprite t = new TranslateSprite(s, 5, 8);

      Assertion.AssertEquals(s.Y + 8, t.Y);
    }

    [Test] public void TestTranslatedSpriteHashCode()
    {
      Sprite s = new Sprite(new Point(10, 13), 6);
      Sprite t = new TranslateSprite(s, 5, 8);

      Assertion.AssertEquals(s.GetHashCode(), t.GetHashCode());
    }
    */
  }
}
