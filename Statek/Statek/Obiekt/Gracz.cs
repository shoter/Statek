using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Damian.MWGK.Obiekt;
using Damian.MWGK.Obiekt.Statek;
using Damian.MWGK.Obiekt.Bron;
using Damian.MWGK.Global.Test;
using Damian.MWGK.Global.Zbiory;
using Damian.MWGK.Global.Graphics;
using Damian.MWGK.Particle.Storage;
namespace Damian.MWGK.Obiekt.Statek.Gracz
{

    struct Bonus
    {
        public bool active;
        public int T;
        public int MaxT;
        public Bonus(int maxt)
        {
            active = false;
            T = 0;
            MaxT = maxt;
        }

        public void Pracuj()
        {
            if (active == false) return;
            T--;
            if (T == 0) active = false;
        }
        public void Aktywuj()
        {
            T = MaxT;
            active = true;
        }
    }


    class Gracz : StatekKosmiczny
    {

        public int IDRakiety;
        public int IDLasera ;
        public int[] LaserGracz = { 0, 1, 2 };
        public int AktLaser = 0;
        ParticleGun Silnik;

        bool Stop = false; //czy stoi w miejscu
        public int IloscPkt = 0;

        public Bonus DoublePoints = new Bonus(450);

            


        public Gracz(Vector2 pozycja,ref Texture2D obrazek,float obrot)
            :base(pozycja, ref obrazek,obrot,100f,1f,50f)
        {

            IDRakiety = 0;
            IDLasera = LaserGracz[AktLaser];
            V = 7f;

            Color start = Color.CadetBlue;
            Color end = start;
            end.A = 0;
            Silnik = new ParticleGun(0, 0, 2, 4, 2, 4,
                start,Color.Blue,end,end,
                -MathHelper.PiOver4, MathHelper.PiOver4,
                60f, 120f, 10, 20, 0, Rodzaj_Ruchu.RuchJednostajny, 7, Particle.Storage.Rodzaj_Dziala.Losowo);
        }

        public void Pracuj(Vector2[] gdzie, int ile, bool laser, bool rakieta)
        {
            if (Zniszczony) return;
            if (ile != 0)
            {
                Dokad = gdzie[0];

                Vector2 dif = Dokad - Pozycja;
                dif.Normalize();
                Normalna = dif;
                Ruch = Rodzaj_Ruchu.RuchJednostajny;
                Stop = false;
            }

            if (Vector2.Distance(Pozycja, Dokad) < 5f) Stop = true;
            if (!Stop)
            {
                Pozycja += Normalna * V;

            }

            Wyposazenie.Lasery[IDLasera].Pracuj();
            Wyposazenie.Rakiety[IDRakiety].Pracuj();

            if(laser)
                if (Wyposazenie.Lasery[IDLasera].Strzel())
                {
                    Laser nowy = new Laser(Pozycja, ref Wyposazenie.Lasery[IDLasera].Pierwowzor.Obrazek, 0f, Wyposazenie.Lasery[IDLasera].Pierwowzor.Sila,
                        new Vector2(0f, -1f), Wyposazenie.Lasery[IDLasera].Pierwowzor.V, Wyposazenie.Lasery[IDLasera].Pierwowzor.KolorLaseru);
                    GlobalAcc.ListaObiektow2.Add(nowy);
                }
            if(rakieta)
                if (Wyposazenie.Rakiety[IDRakiety].Strzel())
                {
                    Rakieta nowa = Wyposazenie.Rakiety[IDRakiety].Pierwowzor.Kopia(Pozycja, Wyposazenie.Rakiety[IDRakiety].Pierwowzor.V, 0f);
                    Random rand = new Random();
                    if (rand.Next(0, 2) == 1) nowa.Niestabilnosc = -nowa.Niestabilnosc;
                    GlobalAcc.ListaObiektow1.Add(nowa);
                }

            Silnik.X = (int)Pozycja.X;
            Silnik.Y = (int)Pozycja.Y;
            Silnik.Obrot = Obrot;
            Silnik.Pracuj(!Zniszczony);
            DoublePoints.Pracuj();


        }


        public override void Rysuj(ref SpriteBatch SB)
        {
            Silnik.Rysuj(ref SB);
            base.Rysuj(ref SB);
            if (DoublePoints.active)
            {
                int ile = (int)((float)DoublePoints.T / DoublePoints.MaxT * 20f) + 1;
                SB.Draw(Grafika.PowerUps[1], new Vector2(460f, 0f), Color.White);
                SB.Draw(Grafika.Empty, new Rectangle(455, (20 - ile), 5, ile), Color.Yellow);
            }
            else
                SB.Draw(Grafika.PowerUps[1], new Vector2(460f, 0f), Color.Gray);
        }
        public override void Czysc()
        {
            Silnik.Czysc(); 
        }


        
    }
}