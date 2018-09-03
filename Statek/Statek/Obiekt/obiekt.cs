using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Damian.MWGK.funkcje;
using System;

namespace Damian.MWGK.Obiekt
{
    enum Rodzaj_Ruchu:byte
    {
        RuchJednostajny=1,
        RuchPrzyspieszony=2,
        RuchOpozniony=3,
        RuchZaden=0
    };

    class Obiekt
    {
        //Tutaj beda podstawowe informacje
        public Vector2 Pozycja;//pozycja lewego gornego rogu statku
        protected Vector2 Dokad; //dokad dany obiekt leci
        public Vector2 rozmiary;
        protected float Odleglosc; //odleglosc od celu ustalona przy pomocy ruchow
        protected Vector2 Srodek; //pozycja srodka statku,uzywana do obracania go
        public float X { set { Pozycja.X = value; } get { return Pozycja.X; } }
        public float Y { set { Pozycja.Y = value; }  get { return Pozycja.Y; } }
        public float A; //jako przyspieszenie,a - acceleration
        public float V;
        public Vector2 Normalna; //normalna ruchu
        public float Obrot { protected set; get /*{ return obrot - MathHelper.PiOver2;}*/; }
        public float zObrot;//zmiana obrotu
        public Texture2D Obrazek;
        public bool Zniszczony = false;
        protected float MaxR;

        //tutaj informacje dodatkowe,tylko gety publiczne

        public Rodzaj_Ruchu Ruch { protected get; set; }
        protected bool BrakStop;
        

        public Obiekt()
        {
            Pozycja = new Vector2(0f,0f);
            Srodek = new Vector2(0f, 0f);
            A = 0f;
            V = 0f;
            zObrot = 0f;

            Obrot = 0f;
        }

        /// <summary>
        /// Konstruktor obiektu,srodek obiektu umieszczany na podstawie tekstury,wazne aby obrazek nie byla NULL 
        /// </summary>
        /// <param name="pozycja">poczatkowa pozycja obiektu</param>
        /// <param name="obrazek">obrazek pod jakim bedzie wyswietlał się obiekt(NIE NULL!!!)</param>
        /// <param name="a">przyspieszenie obiektu</param>
        /// <param name="obrot">obrot obiektu w radianach (0 - 2 PI)</param>
        public Obiekt(Vector2 pozycja,ref Texture2D obrazek,float obrot)
        {
            Pozycja = pozycja;
            Obrot = obrot;
            A = 0f;
            V = 0f;
            zObrot = 0f;

            Obrazek = obrazek;


            rozmiary = new Vector2(Obrazek.Width, Obrazek.Height);
            Vector2 przes = new Vector2(obrazek.Width, obrazek.Height);
            Srodek = (przes / 2);
            MaxR = Math.Max(Obrazek.Width, Obrazek.Height);
        }
        /// <summary>
        /// Konstruktor obiektu,srodek obiektu umieszczany na podstawie tekstury,wazne aby obrazek nei byl pustym stringiem 
        /// </summary>
        /// <param name="pozycja">poczatkowa pozycja obiektu</param>
        /// <param name="obrazek">nazwa tekstury</param>
        /// <param name="CM">Content manager do tego gdzie mamy teksture</param>
        /// <param name="a">przyspieszenie obiektu</param>
        /// <param name="obrot">obrot obiektu w radianach (0 - 2 PI)</param>
        public Obiekt(Vector2 pozycja, string obrazek, ContentManager CM,float obrot)
        {
            Pozycja = pozycja;
            Obrot = obrot;
            Obrazek = CM.Load<Texture2D>(obrazek);

            rozmiary = new Vector2(Obrazek.Width, Obrazek.Height);
            Vector2 przes = rozmiary;
            Srodek = (przes / 2);
        }

        /// <summary>
        /// wykonuje wszystie niezbedne obliczenia dla obiektu
        /// </summary>
        public virtual void Pracuj()
        {
            if (Ruch != Rodzaj_Ruchu.RuchZaden)
            {

                Pozycja += Normalna * V;
                V += A;
                Odleglosc -= V;
                if(!BrakStop)
                if (Odleglosc < 0f || (Ruch == Rodzaj_Ruchu.RuchOpozniony && V < 0f))
                {
                    V = 0f;
                    A = 0f;
                    if (Ruch != Rodzaj_Ruchu.RuchOpozniony)
                        Pozycja = Dokad;
                    else if (Odleglosc < 0f)
                        Pozycja = Dokad;
                    Ruch = Rodzaj_Ruchu.RuchZaden;


                }



            }
            Obrot += zObrot;


        }

        public virtual void Rysuj(ref SpriteBatch SB)
        {
            if(!Zniszczony)
            SB.Draw(Obrazek, Pozycja, null, Color.White, Obrot, Srodek, Vector2.One, SpriteEffects.None, 0);              
        }

        /// <summary>
        /// Nadaje obiektowi predkosc jednostajna
        /// </summary>
        /// <param name="dokad">punkt do jakiego obiekt ma leciec</param>
        /// <param name="v">predkosc z jaka cialo sie porusza</param>
        /// <param name="brakstop">Okresla czy obiekt zatrzyma sie po dotarciu do dokad</param>
        public void RuchJednostajny(Vector2 dokad, float v,bool brakstop)
        {
            Vector2 dif = dokad - Pozycja;
            Odleglosc = dif.Length();
            dif.Normalize();
            Normalna = dif;
            V = v;
            Dokad = dokad;
            Ruch = Rodzaj_Ruchu.RuchJednostajny;
            BrakStop = brakstop;
        }
        /// <summary>
        /// Nadaje obiektowi predkosc przyspieszona
        /// </summary>
        /// <param name="dokad">punkt do jakiego obiekt ma leciec</param>
        /// <param name="v">predkosc z jaka cialo sie porusza</param>
        /// <param name="a">przyspieszenie z jakim objekt sie porusza</param>
        /// <param name="brakstop">Okresla czy obiekt zatrzyma sie po dotarciu do dokad</param>
        public void RuchPrzyspieszony(Vector2 dokad, float v, float a,bool brakstop)
        {
            Vector2 dif = dokad - Pozycja;
            Odleglosc = dif.Length();
            dif.Normalize();
            Normalna = dif;
            V = v;
            A = a;
            Dokad = dokad;
            Ruch = Rodzaj_Ruchu.RuchPrzyspieszony;
            BrakStop = brakstop;
        }

        /// <summary>
        /// Nadaje obiektowi predkosc przyspieszona
        /// </summary>
        /// <param name="dokad">punkt do jakiego obiekt ma leciec</param>
        /// <param name="v">predkosc z jaka cialo sie porusza</param>
        /// <param name="a">oppoznienie z jakim objekt sie porusza(a > 0f)</param>
        /// <param name="brakstop">Okresla czy obiekt zatrzyma sie po dotarciu do dokad</param>
        public void RuchOpozniony(Vector2 dokad, float v, float a, bool brakstop)
        {
            Vector2 dif = dokad - Pozycja;
            Odleglosc = dif.Length();
            dif.Normalize();
            Normalna = dif;
            V = v;
            A = -a;
            Dokad = dokad;
            Ruch = Rodzaj_Ruchu.RuchOpozniony;
            BrakStop = brakstop;
        }

        public bool Kolizja(Obiekt inny)
        {
            if (Funkcje.kolizja(Pozycja - rozmiary / 2, (int)rozmiary.X, (int)rozmiary.Y, inny.Pozycja - inny.rozmiary / 2, (int)inny.rozmiary.X, (int)inny.rozmiary.Y))
                return true;return false;
        }

        public float OdlegloscDo(Obiekt inny)
        {
            return (float)Math.Sqrt(Math.Pow(Pozycja.X - inny.Pozycja.X, 2) + Math.Pow(Pozycja.Y - inny.Pozycja.Y, 2));
        }

        public virtual void ReakcjaKolizja(float sila,Vector2 gdzie)
        {}

        public virtual void Czysc()
        {
            //czysci rzeczy po sobie.
        }
    }
   
}