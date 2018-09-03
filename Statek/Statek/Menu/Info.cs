using Damian.MWGK.Obiekt;
using Damian.MWGK.Global.Test;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Damian.MWGK.Global.Graphics;
namespace Damian.MWGK.GameStates
{
    class Info
    {
        protected static Button PrzyciskExit;

        protected static bool PrzejscieRozpoczete = false;
        protected static int Przejscie = 0; // max 50-0

        public static void init()
        {
            PrzyciskExit = new Button(new Vector2(600f, 400f), ref Grafika.PrzyciskExit, ref Grafika.PrzyciskExit, 0f); //nigdy nie bedzie na poczatku po srodku
            //taki stan jest logicznie niemozliwy chyba ze zmieni sie od wewnatrz zmienne wystepyujace w trakcie gry.


        }

        public static void Pracuj(Vector2[] Klikniecia, int ile)
        {
            PrzyciskExit.Pracuj(Klikniecia, ile);
            if (PrzyciskExit.Pojedyncze_wcisniecie)
                GlobalAcc.StanGry = GameState.ChangingToMenuFromInfoP1;
        }

        public static void PracujPrzejscieDo()
        {
            if (PrzejscieRozpoczete == false)
            {
                PrzyciskExit.RuchPrzyspieszony(new Vector2(500f, PrzyciskExit.Y), 0f, 0.5f, true);
                PrzejscieRozpoczete = true;
                Przejscie = 0;
            }
            else
            {
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
                PrzyciskExit.RuchPrzyspieszony(new Vector2(240f, PrzyciskExit.Y), 0f, 0.5f, false);
                PrzejscieRozpoczete = true;
                Przejscie = 50;
            }
            else
            {
                PrzyciskExit.Pracuj();
                if (Przejscie == 0)
                {
                    GlobalAcc.StanGry = GameState.Info;
                    PrzejscieRozpoczete = false;
                    Przejscie = 0;
                }
                Przejscie--;
            }

        }

        public static void Rysuj(ref SpriteBatch SB)
        {
            SB.Draw(Grafika.InfoOverall, Vector2.Zero, Color.White);
            PrzyciskExit.Rysuj(ref SB);
            if (PrzejscieRozpoczete)
            {
                int Alpha = (int)(Przejscie * 255.0 / 50.0);
                    SB.Draw(Grafika.Empty,new Rectangle(0,0,480,800),new Color(0,0,0,Alpha));
            }
        }


    }

}