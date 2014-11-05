using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BugSmasher
{
    enum BugStates
    {
        Crawling,
        Stopped
    }

    class Bug : Sprite
    {
        public BugStates State;
        public Sprite target;

        public Bug(
           Vector2 location,
           Texture2D texture,
           Rectangle initialFrame,
           Vector2 velocity) : base (location, texture, initialFrame, velocity)
        {
            State = BugStates.Crawling;

        }

        public override void Update(GameTime gameTime)
        {
            if (target != null)
            {
                
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
