using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BugSmasher
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;
        Texture2D spriteSheet;
        Random rand = new Random();
        float bloopTime = 1.0f;
        float remainTime = 0.0f;
        List<Bug> bugs = new List<Bug>();
        Sprite Cursor;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("background");
            spriteSheet = Content.Load<Texture2D>("spritesheet");

            // TODO: use this.Content to load your game content here
            
            Cursor = new Sprite(new Vector2(40, 40), spriteSheet, new Rectangle(137, 198, 44, 53), Vector2.Zero);
            Spawnbug1(new Vector2(rand.Next(0, 18), rand.Next(0, 450)), new Vector2(80, 0));

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void Spawnbug1(Vector2 location, Vector2 velocity)
        {
            Bug brbug = new Bug(location, spriteSheet, new Rectangle(6, 15, 53, 32), velocity);
            //brbug.state = BugStates.Stopped;

            bugs.Add(brbug);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState CurrentKeyboardState = Keyboard.GetState();
            MouseState ms = Mouse.GetState();
            // Allows the game to exit
            if (CurrentKeyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            Cursor.Update(gameTime);
            Cursor.Location = new Vector2(ms.X - 10, ms.Y - 15);
            Cursor.Velocity = new Vector2(ms.X, ms.Y);

            //Vector2 location = new Vector2(rand.Next(0, 18), rand.Next(0, 450));

            
            Vector2 velocity = new Vector2(rand.Next(-130, 130), rand.Next(-25, 25));

            

            for (int i = bugs.Count - 1; i >= 0; i--)
            {
                bugs[i].Update(gameTime);

                velocity = new Vector2(rand.Next(100, 500), rand.Next(80, 450)) - bugs[i].Location;
                velocity.Normalize();
                velocity *= 40;

                bugs[i].FlipHorizontal = false;
                bugs[i].Velocity = velocity;

                if (ms.LeftButton == ButtonState.Pressed && Cursor.IsBoxColliding(bugs[i].BoundingBoxRect))
                {
                    bugs.RemoveAt(i);
                }
                
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            for (int c = 0; c < bugs.Count; c++)
            {
                bugs[c].Draw(spriteBatch);
            }    
       
            Cursor.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
