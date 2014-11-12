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
        Stopped,
        Waiting,
        Done,
        Clickable
    }

    class Bug : Sprite
    {
        public BugStates State;
        public bool Dead = false;

        private Random rand = new Random((int)DateTime.UtcNow.Ticks);

        private float moveTimer = 0;
        private float moveTimerMax = 180f;
        private float deadTimer = 0;
        private float deadTimerMax = 3500f;

        public Bug(
           Vector2 location,
           Texture2D texture,
           Rectangle initialFrame,
           Vector2 velocity) : base (location, texture, initialFrame, velocity)
        {
            State = BugStates.Crawling & BugStates.Clickable;
            System.Threading.Thread.Sleep(1);
        }

        public void Splat()
        {
            this.frames[0] = new Rectangle(0, 128, 128, 128);
            this.Velocity = Vector2.Zero;
            this.Dead = true;
            this.Fade = true;
        }

        public override void Update(GameTime gameTime)
        {
            moveTimer += (float)gameTime.ElapsedGameTime.Milliseconds;

            if (State == BugStates.Done)
            { 
                if(
            }


            if (Dead)
                deadTimer += (float)gameTime.ElapsedGameTime.Milliseconds;

            if (!Dead)
            {
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

                this.TintColor = Color.White;

                if (this.State == BugStates.Waiting)
                {
                    this.velocity /= 4;
                    this.TintColor = Color.Red;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (deadTimer < deadTimerMax)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}
