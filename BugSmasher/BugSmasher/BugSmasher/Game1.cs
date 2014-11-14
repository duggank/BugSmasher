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
        Texture2D gameOver;
        Texture2D Fire;
        Texture2D gunnygoo;
        Random rand = new Random();
        List<Bug> bugs = new List<Bug>();
        Sprite Cursor;
        Sprite Bar;
        Sprite GO;
        Sprite Progress;
        Sprite Progress2;
        Sprite Flame;
        Sprite Thrower;
        bool Shoot = false;
        bool Cool = false;
        bool GameOver = false;
        bool Hand = true;
        bool Gun = false;

        float ptime = 0.0f;
        float maxptime = 8.25f * 1000f;

        int Score = 0;
        int barWidth = 0;
        int barWidthMax = 365;

        int userClicked = 0;
        Vector2 userClickLocation = Vector2.Zero;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
            Window.Title = "Score: ";

            //System.Diagnostics.Process.Start("http://www.StackOverflow.com");
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
            gameOver = Content.Load<Texture2D>("gameover");
            Fire = Content.Load<Texture2D>("Fire");
            gunnygoo = Content.Load<Texture2D>("Gun");

            // TODO: use this.Content to load your game content here
            
            Cursor = new Sprite(new Vector2(40, 40), spriteSheet, new Rectangle(137, 198, 44, 53), Vector2.Zero);
            Bar = new Sprite(new Vector2(180, 0), spriteSheet, new Rectangle(2, 300, 462, 82), Vector2.Zero);
            Progress = new Sprite(new Vector2(210, 22), spriteSheet, new Rectangle(1, 384, 27, 41), Vector2.Zero);
            Progress2 = new Sprite(new Vector2(238, 22), spriteSheet, new Rectangle(31, 384, 10, 39), Vector2.Zero);
            GO = new Sprite(new Vector2(238, 200), gameOver, new Rectangle(26, 3, 284, 269), Vector2.Zero);
            Flame = new Sprite(new Vector2(200, 200), Fire, new Rectangle(230, 54, 132, 75), Vector2.Zero);
            Thrower = new Sprite(new Vector2(200, 200), gunnygoo, new Rectangle(144, 65, 77, 53), Vector2.Zero);
            ScoreUpdate();

            

            for (int col = 0; col < 15; col++)
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
            if (Cool && (Score >= 28 && Score < 40))
                Window.Title = "Score: " + Score + "    You did okay...";
            if (Cool && (Score >= 40))
                Window.Title = "Score: " + Score + "    YOU DID AWESOME!!! :D";
            
        }

        public void Spawnbug1(Vector2 location, Vector2 velocity)
        {
            Rectangle rect = new Rectangle(rand.Next(0, 3) * 64, rand.Next(0, 2) * 64, 64, 64);

            Bug brbug = new Bug(location, spriteSheet, rect, velocity);
            //brbug.state = BugStates.Stopped;
            bugs.Add(brbug);
        }

        public void GameOverTime()
        {
            if (GameOver)
            {
                for (int l = 0; l < bugs.Count; l++)
                {
                    bugs[l].State = BugStates.Done;
                }
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            ptime += (float)gameTime.ElapsedGameTime.Milliseconds;
            barWidth = (int)((ptime / maxptime) * 300);
            
                if (barWidth == barWidthMax && Score < 28)
                {
                    GameOver = true;
                    GameOverTime();
                }

            if (barWidth > barWidthMax)
            {
                barWidth = barWidthMax;
                Cool = true;
            }

            if (barWidth == barWidthMax && (Score < 40 && Score >= 28))
                ScoreUpdate();

            if (barWidth == barWidthMax && (Score >= 40))
                ScoreUpdate();



            

            KeyboardState CurrentKeyboardState = Keyboard.GetState();
            if(CurrentKeyboardState.IsKeyDown(Keys.D2))
            {
                Hand = false;
                Gun = true;
            }
            else if (CurrentKeyboardState.IsKeyDown(Keys.D1))
            {
                Hand = true;
                Gun = false;
            }

            MouseState ms = Mouse.GetState();
            // Allows the game to exit
            if (CurrentKeyboardState.IsKeyDown(Keys.Escape))
                this.Exit();
            Flame.Update(gameTime);
            Thrower.Update(gameTime);
            Bar.Update(gameTime);
            Progress.Update(gameTime);
            GO.Update(gameTime);

            Cursor.Update(gameTime);
            Cursor.Location = new Vector2(ms.X - 16, ms.Y - 15);           
            Cursor.Velocity = new Vector2(ms.X, ms.Y);
            Thrower.Location = new Vector2(ms.X - 10, ms.Y - 10);
            Thrower.Velocity = new Vector2(ms.X, ms.Y);
            Flame.Location = new Vector2(ms.X - 120, ms.Y - 30);
            Flame.Velocity = new Vector2(ms.X, ms.Y);

            //Vector2 location = new Vector2(rand.Next(0, 18), rand.Next(0, 450));

            
            Vector2 velocity = new Vector2(rand.Next(-130, 130), rand.Next(-25, 25));
    
            if (ms.LeftButton == ButtonState.Pressed && userClicked == 0 && Hand)
            {
                userClicked = 1;
                userClickLocation = new Vector2(ms.X, ms.Y);
            }

            if (ms.LeftButton == ButtonState.Released && Hand)
                userClicked = 0;

            else if(ms.LeftButton == ButtonState.Pressed && Gun)
                 {
                     Shoot = true;
                 }
            if (ms.LeftButton == ButtonState.Released && Gun)
            {
                Shoot = false;
            }

            for (int i = bugs.Count - 1; i >= 0; i--)
            {
                bugs[i].Update(gameTime);

                float clickDist = Vector2.Distance(userClickLocation, bugs[i].Center);

                
                    if (userClicked == 1 && !bugs[i].Dead && clickDist < 50 && !GameOver && !Cool && Hand)
                    {
                        //bugs.RemoveAt(i);
                        Score += 1;
                        ScoreUpdate();
                        bugs[i].Splat();

                        userClicked = 2;
                    }
                    if (ms.LeftButton == ButtonState.Pressed && Gun && Flame.IsBoxColliding(bugs[i].BoundingBoxRect) && !bugs[i].Dead)
                    {
                        Score += 1;
                        ScoreUpdate();
                        bugs[i].Splat();
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

            for (int k = 0; k < 100; k++)
            {
                if (barWidth == barWidthMax && Score < 28)
                {
                    GO.Draw(spriteBatch);
                }
            }

            Bar.TintColor = new Color(1, 1, 1, 0.6f);
            Bar.Draw(spriteBatch);
            Progress.Draw(spriteBatch);
            for (int k = 0; k < 100; k++)
            {
                if(Hand && !Gun)
                   Cursor.Draw(spriteBatch);
                if (!Hand && Gun)
                {
                    Thrower.Draw(spriteBatch);
                    if (Shoot)
                        Flame.Draw(spriteBatch);
                }
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);

            spriteBatch.Draw(spriteSheet, new Rectangle(235, 22, barWidth, 39), new Rectangle(32, 384, 6, 39), Color.White);
            spriteBatch.Draw(spriteSheet, new Rectangle(235 + barWidth, 22, 16, 39), new Rectangle(64, 384, 16, 39), Color.White);
            

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
