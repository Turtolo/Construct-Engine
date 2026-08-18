using System;
using System.Collections.Generic;

namespace Opal.Tools
{
  public class Anchor : Token
  {
    public List<Token> Chained { get; private set; } = new();
    
    public void Activate()
    {
      //token.setCurrentAnchorPoint(this);
    }

    public void Attach(Token t)
    {
      Chained.Add(t);
    }

    public void Detach(Token t)
    {
      Chained.Remove(t);
    }

    public void Dispose()
    {
      for (int i = Chained.Count - 1; i >= 0; i--)
      {
        Token t = Chained[i];

        Chained.RemoveAt(i);
        t.QueueFree();
      }

      QueueFree();
    }
  }
}
