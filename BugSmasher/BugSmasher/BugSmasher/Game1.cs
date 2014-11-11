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
        List<Bug> bugs = new List<Bug>();
        Sprite Cursor;
        Sprite Bar;
        Sprite Progress;
        Sprite Progress2;

        float ptime = 0.0f;
        float maxptime = 20.0f;

        int Score = 0;

        int userClicked = 0;
        Vector2 userClickLocation = Vector2.Zero;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
            Window.Title = "Score: ";
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
            this.IsMouseVisible = true;

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
            Bar = new Sprite(new Vector2(180, 0), spriteSheet, new Rectangle(2, 300, 462, 82), Vector2.Zero);
            Progress = new Sprite(new Vector2(210, 22), spriteSheet, new Rectangle(1, 384, 27, 41), Vector2.Zero);
            Progress2 = new Sprite(new Vector2(238, 22), spriteSheet, new Rectangle(31, 384, 10, 39), Vector2.Zero);
            ScoreUpdate();

            Spawnbug1(new Vector2(rand.Next(0, 18), rand.Next(0, 10)), new Vector2(80, 0));
            Spawnbug1(new Vector2(rand.Next(0, 18), rand.Next(30, 40)), new Vector2(80, 0));

            for (int col = 0; col < 10; col++)
            {
                for (int row = 0; row < 4; row++)
                {
                    Spawnbug1(new Vector2(-400 + 64 * col + rand.Next(-32, 32), 140 + row * 90 + rand.Next(-64, 64)), new Vector2(80, 0));
                }
            }

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void ScoreUpdate() 
        {
            Window.Title = "Score: " + Score;
        }

        public void Spawnbug1(Vector2 location, Vector2 velocity)
        {
            Rectangle rect = new Rectangle(rand.Next(0, 3) * 64, rand.Next(0, 2) * 64, 64, 64);

            Bug brbug = new Bug(location, spriteSheet, rect, velocity);
            //brbug.state = BugStates.Stopped;
            bugs.Add(brbug);
        }
        public void Spawnbug2(Vector2 location, Vector2 velocity) 
        {
            
            Bug spbug = new Bug(new Vector2(rand.Next(0, 5), rand.Next(30, 100)), spriteSheet, new Rectangle(72, 9, 52, 41), velocity);
            bugs.Add(spbug);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            ptime += (float)gameTime.ElapsedGameTime.Seconds;
                Progress2.Update(gameTime);

            KeyboardState CurrentKeyboardState = Keyboard.GetState();
            MouseState ms = Mouse.GetState();
            // Allows the game to exit
            if (CurrentKeyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            Bar.Update(gameTime);
            Progress.Update(gameTime);

            Cursor.Update(gameTime);
            Cursor.Location = new Vector2(ms.X - 10, ms.Y - 15);
            Cursor.Velocity = new Vector2(ms.X, ms.Y);

            //Vector2 location = new Vector2(rand.Next(0, 18), rand.Next(0, 450));

            
            Vector2 velocity = new Vector2(rand.Next(-130, 130), rand.Next(-25, 25));

            if (ms.LeftButton == ButtonState.Pressed && userClicked == 0)
            {
                userClicked = 1;
                userClickLocation = new Vector2(ms.X, ms.Y);
            }

            if (ms.LeftButton == ButtonState.Released)
                userClicked = 0;

            for (int i = bugs.Count - 1; i >= 0; i--)
            {
                bugs[i].Update(gameTime);

                float clickDist = Vector2.Distance(userClickLocation, bugs[i].Center);

                if (userClicked == 1 && !bugs[i].Dead && clickDist < 50)
                {
                    //bugs.RemoveAt(i);
                    Score += 1;
                    ScoreUpdate();
                    bugs[i].Splat();

                    userClicked = 2;
                }


                bugs[i].State = BugStates.Crawling;

                for (int j = 0; j < bugs.Count; j++)
                {
                    if (i == j || bugs[i].Dead || bugs[j].Dead)
                        continue;

                    float dist = Vector2.Distance(bugs[i].Center, bugs[j].Center);

                    if (dist < 50 && bugs[i].Center.X < bugs[j].Center.X)
                    {
                        bugs[i].State = BugStates.Waiting;
                    }
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
                if (bugs[c].Dead)
                    bugs[c].Draw(spriteBatch);
            }

            for (int c = 0; c < bugs.Count; c++)
            {
                if (!bugs[c].Dead)
                    bugs[c].Draw(spriteBatch);
            }

            Bar.TintColor = new Color(1, 1, 1, 0.6f);
            Bar.Draw(spriteBatch);
            Progress.Draw(spriteBatch);
            Progress2.Draw(spriteBatch);
            Cursor.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
