using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Damian.MWGK.Obiekt;
using Microsoft.Xna.Framework.Graphics;
using Damian.MWGK.Sprite;
using Damian.MWGK.Obiekt.Statek.Gracz;
using Damian.MWGK.Fala;
using Damian.MWGK.Global.Graphics;
using Damian.MWGK.Obiekt.Powers;
using Damian.MWGK.GameStates;
namespace Damian.MWGK.Global.Test
{
    class GlobalAcc
    {
        public static int Czasteczki, Obiekty;
        public static List<Obiekt.Obiekt> ListaKolizji = new List<Obiekt.Obiekt>();
        public static List<Obiekt.Obiekt> ListaObiektow1 = new List<Obiekt.Obiekt>();//z przodu
        public static List<Obiekt.Obiekt> ListaObiektow2 = new List<Obiekt.Obiekt>();//po srodku
        public static List<Obiekt.Obiekt> ListaObiektow3 = new List<Obiekt.Obiekt>();//tyl
        public static List<SpriteAnimated> ListaAnimacji1 = new List<SpriteAnimated>();
        public static List<SpriteAnimated> ListaAnimacji2 = new List<SpriteAnimated>();
        public static List<SpriteAnimated> ListaAnimacji3 = new List<SpriteAnimated>();
        public static List<FalaWrogow> ListaWrogow = new List<FalaWrogow>();
        public static List<PowerUp> ListaBonusow = new List<PowerUp>();
        public static GameState StanGry = GameState.Menu;



        public static Gracz StatekGracza;

        //TESTOWO KURKA!
        public static void reset()
        {

            Czasteczki = 0;
            Obiekty = 0;
        }

        public static void Init()
        {
           GlobalAcc.StatekGracza = new Gracz(new Vector2(200f, 600f), ref Grafika.StatekGracza, 0f);
           
        }

        public static int czest = 1; //co 10 bedzie sprawdzal czy usunac nieuzywane skladniki z list

        public static void Pracuj()
        {
            foreach (Obiekt.Obiekt X in ListaObiektow1)
                    X.Pracuj();
            foreach (Obiekt.Obiekt X in ListaObiektow2)
                X.Pracuj();
            foreach (Obiekt.Obiekt X in ListaObiektow3)
                X.Pracuj();

            foreach (SpriteAnimated X in ListaAnimacji1)
                X.Pracuj();
            foreach (SpriteAnimated X in ListaAnimacji2)
                X.Pracuj();
            foreach (SpriteAnimated X in ListaAnimacji3)
                X.Pracuj();
            foreach (FalaWrogow X in ListaWrogow)
                X.Pracuj();
            foreach (PowerUp X in ListaBonusow)
                X.Pracuj();

            if ( (czest %= 10) == 0)
            {
                //co 10 cykli probuje usunac obiekty
           for(int i = 0;i < ListaObiektow1.Count;i++)
               if (ListaObiektow1[i].Pozycja.X < -100f || ListaObiektow1[i].Pozycja.X > 480f ||
                   ListaObiektow1[i].Pozycja.Y < -100f || ListaObiektow1[i].Pozycja.Y > 900f)
                   ListaObiektow1.RemoveAt(i);
           for (int i = 0; i < ListaObiektow2.Count; i++)
               if (ListaObiektow2[i].Pozycja.X < -100f || ListaObiektow2[i].Pozycja.X > 480f ||
                   ListaObiektow2[i].Pozycja.Y < -100f || ListaObiektow2[i].Pozycja.Y > 900f)
                   ListaObiektow2.RemoveAt(i);
           for (int i = 0; i < ListaObiektow3.Count; i++)
               if (ListaObiektow3[i].Pozycja.X < -100f || ListaObiektow3[i].Pozycja.X > 480f ||
                   ListaObiektow3[i].Pozycja.Y < -100f || ListaObiektow3[i].Pozycja.Y > 900f)
                   ListaObiektow3.RemoveAt(i);
           for (int i = 0; i < ListaWrogow.Count; i++)
               if (ListaWrogow[i].Koniec)
                   ListaWrogow.RemoveAt(i);


           for (int i = 0; i < ListaKolizji.Count; i++)
               if (ListaKolizji[i].Zniszczony == true) ListaKolizji.RemoveAt(i);

           for (int i = 0; i < ListaAnimacji1.Count; i++)
               if (ListaAnimacji1[i].Koniec == true) ListaAnimacji1.RemoveAt(i);

           for (int i = 0; i < ListaAnimacji2.Count; i++)
               if (ListaAnimacji2[i].Koniec == true) ListaAnimacji2.RemoveAt(i);

           for (int i = 0; i < ListaAnimacji3.Count; i++)
               if (ListaAnimacji3[i].Koniec == true) ListaAnimacji3.RemoveAt(i);

           for (int i = 0; i < ListaBonusow.Count; i++)
               if (ListaBonusow[i].Zniszczony)
                   ListaBonusow.RemoveAt(i);

            }
            czest++;
        }

        public static void Rysuj(ref SpriteBatch SB)
        {
            foreach (SpriteAnimated X in ListaAnimacji1)
                X.Rysuj(ref SB);
            foreach (Obiekt.Obiekt X in ListaObiektow1)
                    X.Rysuj(ref SB);
            
            foreach (SpriteAnimated X in ListaAnimacji2)
                X.Rysuj(ref SB);
            foreach (FalaWrogow X in ListaWrogow)
                X.Rysuj(ref SB);
            foreach (Obiekt.Obiekt X in ListaObiektow2)
                X.Rysuj(ref SB);

            foreach (SpriteAnimated X in ListaAnimacji3)
                X.Rysuj(ref SB);
            foreach (Obiekt.Obiekt X in ListaObiektow3)
                X.Rysuj(ref SB);
            foreach (PowerUp X in ListaBonusow)
                X.Rysuj(ref SB);
            
        }

        public static void Clear()
        {
            ListaAnimacji1.Clear();
            ListaAnimacji2.Clear();
            ListaAnimacji3.Clear();
            ListaObiektow1.Clear();
            ListaObiektow2.Clear();
            ListaObiektow3.Clear();
            ListaWrogow.Clear();
            ListaKolizji.Clear();
            ListaBonusow.Clear();
        }

        


    }
}