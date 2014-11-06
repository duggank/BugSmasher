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

        private Random rand = new Random((int)DateTime.UtcNow.Ticks);

        private float moveTimer = 0;
        private float moveTimerMax = 280f;

        public Bug(
           Vector2 location,
           Texture2D texture,
           Rectangle initialFrame,
           Vector2 velocity) : base (location, texture, initialFrame, velocity)
        {
            State = BugStates.Crawling;
            System.Threading.Thread.Sleep(1);
        }

        public override void Update(GameTime gameTime)
        {
            moveTimer += (float)gameTime.ElapsedGameTime.Milliseconds;

            if (moveTimer > moveTimerMax)
            {
                moveTimer = 0;

                velocity = new Vector2(Center.X + 200, Center.Y + rand.Next(-165, 170)) - Location;
                velocity.Normalize();
                velocity *= 40;
                Rotation = (float)Math.Atan2(velocity.Y, velocity.X);

                FlipHorizontal = false;
                Velocity = velocity;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
