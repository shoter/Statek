using Damian.MWGK.Obiekt;
using Damian.MWGK.Global.Test;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Damian.MWGK.Global.Graphics;
namespace Damian.MWGK.GameStates
{
    class Menu
    {
        protected static Button PrzyciskGra;
        protected static Button PrzyciskInfo;
        public static Button PrzyciskExit;
        protected static Button PrzyciskHelp;

        protected static bool PrzejscieRozpoczete = false;
        protected static int Przejscie = 0; // max 50-0

        public static void init()
        {
            PrzyciskGra = new Button(new Vector2(240f, 100f), ref Grafika.PrzyciskGraj, ref Grafika.PrzyciskGraj, 0f);
            PrzyciskInfo = new Button(new Vector2(240f, 200f), ref Grafika.PrzyciskInfo, ref Grafika.PrzyciskInfo, 0f);
            PrzyciskHelp = new Button(new Vector2(240f, 300f), ref Grafika.PrzyciskHelp, ref Grafika.PrzyciskHelp, 0f);
            PrzyciskExit = new Button(new Vector2(240f, 400f), ref Grafika.PrzyciskExit, ref Grafika.PrzyciskExit, 0f);


        }

        public static void Pracuj(Vector2[] Klikniecia, int ile)
        {
            PrzyciskGra.Pracuj(Klikniecia, ile);
            PrzyciskInfo.Pracuj(Klikniecia, ile);
            PrzyciskExit.Pracuj(Klikniecia, ile);
            PrzyciskHelp.Pracuj(Klikniecia, ile);
            if (PrzyciskGra.Pojedyncze_wcisniecie)
                GlobalAcc.StanGry = GameState.ChangingToGameFromMenuP1;
            if(PrzyciskInfo.Pojedyncze_wcisniecie)
                GlobalAcc.StanGry = GameState.ChangingToInfoFromMenuP1;
            if (PrzyciskHelp.Pojedyncze_wcisniecie)
                GlobalAcc.StanGry = GameState.ChangingToHelpFromMenuP1;
        }

        public static void PracujPrzejscieDo()
        {
            if (PrzejscieRozpoczete == false)
            {
                PrzyciskGra.RuchPrzyspieszony(new Vector2(-100f, PrzyciskGra.Y),  0f, 0.5f, true);
                PrzyciskInfo.RuchPrzyspieszony(new Vector2(-100f, PrzyciskInfo.Y), 0f, 0.5f, true);
                PrzyciskHelp.RuchPrzyspieszony(new Vector2(-100f, PrzyciskHelp.Y), 0f, 0.5f, true);
                PrzyciskExit.RuchPrzyspieszony(new Vector2(-100f, PrzyciskExit.Y), 0f, 0.5f, true);
                PrzejscieRozpoczete = true;
                Przejscie = 0;
            }
            else
            {
                PrzyciskGra.Pracuj();
                PrzyciskInfo.Pracuj();
                PrzyciskHelp.Pracuj();
                PrzyciskExit.Pracuj();
                if (Przejscie == 50)
                {
                    GlobalAcc.StanGry += 1;
                    PrzejscieRozpoczete = false;
                    Przejscie = 0;
                }
                    Przejscie++;
            }
        }
        public static void PracujPrzejscieZ()
        {
            if (PrzejscieRozpoczete == false)
            {
                PrzyciskGra.RuchPrzyspieszony(new Vector2(240f, PrzyciskGra.Y), 0f, 0.5f, false);
                PrzyciskInfo.RuchPrzyspieszony(new Vector2(240f, PrzyciskInfo.Y), 0f, 0.5f, false);
                PrzyciskHelp.RuchPrzyspieszony(new Vector2(240f, PrzyciskHelp.Y), 0f, 0.5f, false);
                PrzyciskExit.RuchPrzyspieszony(new Vector2(240f, PrzyciskExit.Y), 0f, 0.5f, false);
                PrzejscieRozpoczete = true;
                Przejscie = 50;
            }
            else
            {
                PrzyciskGra.Pracuj();
                PrzyciskInfo.Pracuj();
                PrzyciskHelp.Pracuj();
                PrzyciskExit.Pracuj();
                if (Przejscie == 0)
                {
                    GlobalAcc.StanGry = GameState.Menu;
                    PrzejscieRozpoczete = false;
                    Przejscie = 0;
                }
                Przejscie--;
            }

        }

        public static void Rysuj(ref SpriteBatch SB)
        {
            PrzyciskGra.Rysuj(ref SB);
            PrzyciskInfo.Rysuj(ref SB);
            PrzyciskHelp.Rysuj(ref SB);
            PrzyciskExit.Rysuj(ref SB);
            if (PrzejscieRozpoczete)
            {
                int Alpha = (int)(Przejscie * 255.0 / 50.0);
                SB.Draw(Grafika.Empty, new Rectangle(0, 0, 480, 800), new Color(0, 0, 0, Alpha));
            }
        }


    }

}