using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Damian.MWGK.Obiekt;
using Damian.MWGK.Button;
using Damian.MWGK.Particle;
using Damian.MWGK.Particle.Storage;
using Damian.MWGK.Particle.System;
using Damian.MWGK.Obiekt.Bron;
using Damian.MWGK.Global.Test;
using Damian.MWGK.Obiekt.Statek;
using Damian.MWGK.Dodatki.Gwiazdy;
using Damian.MWGK.Obiekt.Statek.Gracz;
using Damian.MWGK.Sprite;
using Damian.MWGK.Global.Graphics;
using Damian.MWGK.Fala;
using Damian.MWGK.Global.Zbiory;
using Damian.MWGK.GameStates;
namespace Statek
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Vector2 pozycja = new Vector2(400, 50);
        float deltaFPSTime = 0;
        SpriteFont TestowyFont;
        string iloscfps="NaN";
        Obiekt kolizja;

        FalaInformacyjna FI;
        FalaWrogow FW;
         

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            graphics.SupportedOrientations = DisplayOrientation.Portrait;
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
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
            this.IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;

            Particle.inicjalizuj(GraphicsDevice);

            ParticleSystem.Inicjalizuj();



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

            Random rand = new Random();
            

            Grafika.Init(Content,this.GraphicsDevice);
            GlobalAcc.Init();
            Wyposazenie.Inicjalizacja(Content);
            Mapy.Init(this.GraphicsDevice);

            TestowyFont = Content.Load<SpriteFont>("testfont");


                




    

            float[] pozycjeX = {100,100,400,400,100};
            float[] pozycjeY = {100,300,300,100,100};
            float [] czas = {0f,20f,70f,100f,120f};
            CurveKey []KurwyX = new CurveKey[5];
            CurveKey []KurwyY = new CurveKey[5];
            for(int i = 0;i < 5;i++)
            {
                KurwyX[i] = new CurveKey(czas[i],pozycjeX[i]);
                KurwyY[i] = new CurveKey(czas[i],pozycjeY[i]);
            }
            kolizja = new Obiekt(new Vector2(750f, 320f), "GuzikN", Content, 0f);
            GlobalAcc.ListaKolizji.Add(kolizja);
            GlobalAcc.ListaObiektow3.Add(kolizja);
            

            SpriteAnimated SA = new SpriteAnimated(300, 300, Grafika.Eksplozja3, 60);
            GlobalAcc.ListaAnimacji1.Add(SA);



  
            





            // TODO: use this.Content to load your game content here
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (gameTime.IsRunningSlowly)
            {

            }
            TouchCollection TC = TouchPanel.GetState();
            Vector2[] klikniecia = new Vector2[4];
            int i = 0;
            foreach (TouchLocation TL in TC)
            {
                if (TL.State == TouchLocationState.Moved)
                {
                    klikniecia[i++] = TL.Position;
                }
            }


            ParticleSystem.Pracuj();
            if(GlobalAcc.StanGry == GameState.Game)
            Mapy.Pracuj(klikniecia,i);


            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            deltaFPSTime += elapsed;
            if (deltaFPSTime > 1)
            {
                float fps = 1 / elapsed;
                iloscfps = "<" + fps.ToString() + ">";
                deltaFPSTime -= 1;
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            if (GlobalAcc.StanGry == GameState.Game)
            {
                Mapy.Rysuj(ref spriteBatch);
                spriteBatch.DrawString(TestowyFont, GlobalAcc.StatekGracza.IloscPkt.ToString(), new Vector2(300f, 0), Color.White);
            }
            if (GlobalAcc.StanGry == GameState.Menu)

            spriteBatch.DrawString(TestowyFont, iloscfps, Vector2.Zero, Color.White);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
