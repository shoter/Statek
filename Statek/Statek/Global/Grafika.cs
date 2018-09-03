using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Damian.MWGK.Sprite;
namespace Damian.MWGK.Global.Graphics
{
    class Grafika
    {
        public static MultiSprite Eksplozja1;
        public static MultiSprite Eksplozja3;
        public static Texture2D StatekGracza;
        public static Texture2D[] Statki = new Texture2D[5];

        public static Texture2D[] Planety = new Texture2D[8];
        public static Vector2[] PolozeniePlanet = new Vector2[8];

        public static Texture2D[] PowerUps = new Texture2D[10];

        public static Texture2D[] Bossowie = new Texture2D[1];

        public static Texture2D Empty;

        public static SpriteFont FontNazwaLvl;

        public static Texture2D PrzyciskGraj;
        public static Texture2D PrzyciskInfo;
        public static Texture2D PrzyciskExit;
        public static Texture2D PrzyciskHelp;

        public static Texture2D HelpOverall, InfoOverall;

        public static void Init(ContentManager CM,GraphicsDevice GD)
        {
            Texture2D expl = CM.Load<Texture2D>("Explode1");
            Texture2D exp3 = CM.Load<Texture2D>("Explode3");
            Eksplozja1 = new MultiSprite(ref expl, 23, 23, 8, 1, 1, 1);
            Eksplozja3 = new MultiSprite(ref exp3, 49, 49, 4, 2, 0, 0);

            for (int i = 0; i < 8; i++)
                Planety[i] = CM.Load<Texture2D>("Planety\\planet" + (i + 1).ToString());
            PolozeniePlanet[0] = new Vector2(270f, 510f);
            PolozeniePlanet[1] = new Vector2(0f, 600f);

            PowerUps[0] = CM.Load<Texture2D>("PowerUps\\bonustest");
            PowerUps[1] = CM.Load<Texture2D>("PowerUps\\Double");
            PowerUps[2] = CM.Load<Texture2D>("PowerUps\\Laser");
            PowerUps[3] = CM.Load<Texture2D>("PowerUps\\PointBlue");
            PowerUps[4] = CM.Load<Texture2D>("PowerUps\\PointBrown");
            PowerUps[5] = CM.Load<Texture2D>("PowerUps\\PointGreen");
            PowerUps[6] = CM.Load<Texture2D>("PowerUps\\PointRed");

            StatekGracza = CM.Load<Texture2D>("Ship");
            Statki[0] = CM.Load<Texture2D>("Ship2");

            Bossowie[0] = CM.Load<Texture2D>("Boss");

            FontNazwaLvl = CM.Load<SpriteFont>("testfont");

            PrzyciskGraj = CM.Load<Texture2D>("ButtonGra");
            PrzyciskInfo = CM.Load<Texture2D>("Opcje");
            PrzyciskExit = CM.Load<Texture2D>("Exit");
            PrzyciskHelp = CM.Load<Texture2D> ("Help");

            HelpOverall = CM.Load<Texture2D>("HelpOverall");
            InfoOverall = CM.Load<Texture2D>("InfoOverall");

            Empty = new Texture2D(GD, 1, 1);
            Color[] pixels = new Color[1];
            pixels[0] = Color.White;
            Empty.SetData<Color>(pixels);
        }
    }
}