using System;
using System.Collections.Generic;
using Damian.MWGK.Obiekt.Statek;
using Damian.MWGK.Obiekt.Statek.Gracz;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Damian.MWGK.Global.Test;
using Damian.MWGK.Obiekt.Bron;
using Damian.MWGK.Global.Zbiory;
using Damian.MWGK.Obiekt.Powers;
using Damian.MWGK.Particle.Storage;

namespace Damian.MWGK.Obiekt.Statek
{
    class Boss : StatekWroga
    {
        public new DzialoLaserowe[] BronLaser;
        public new DzialoRakietowe[] BronRakieta;
        public new bool[] LaserNaprowadzany;
        public new bool[] RakietyNaprowadzane;
        public Vector2[] PozycjaLasera;
        public Vector2[] PozycjaRakiet;
        public bool[] LaserStrzelaj;
        public bool[] RakietyStrzelaj;

        public Boss(Vector2 pozycja, ref Texture2D obrazek, float obrot, float tarcza, float regtarcza, float kadlub, DzialoLaserowe[] bronielaserowe, DzialoRakietowe[] bronierakietowe,
            bool[] lasernapr, bool[] rakietanaprowadzana,Vector2[] pozycjalasera,Vector2[] pozycjarakieta, int pkt)
            : base(pozycja,ref obrazek, obrot, tarcza, regtarcza, kadlub, -1, -1, false, false, pkt, null, 0, null)
        {
            BronLaser = bronielaserowe;
            BronRakieta = bronierakietowe;
            LaserNaprowadzany = lasernapr;
            RakietyNaprowadzane = rakietanaprowadzana;
            PozycjaLasera = pozycjalasera;
            if(PozycjaLasera != null)
            for (int i = 0; i < PozycjaLasera.Length; i++)
            {
                PozycjaLasera[i].X -= rozmiary.X / 2;
                PozycjaLasera[i].Y -= rozmiary.Y / 2;
            }
            PozycjaRakiet = pozycjarakieta;
            if (PozycjaRakiet != null)
            for (int i = 0; i < PozycjaRakiet.Length; i++)
            {
                PozycjaRakiet[i].X -= rozmiary.X / 2;
                PozycjaRakiet[i].Y -= rozmiary.Y / 2;
            }
        }

        public override void Pracuj(bool laser,bool rakieta)
        {
            base.Pracuj();
            for (int i = 0; i < BronLaser.Length; i++)
            {
                BronLaser[i].Pracuj();
                if(laser)
                if (LaserStrzelaj[i])
                if (BronLaser[i].Strzel()) 
                    if (LaserNaprowadzany[i])
                    {
                        float ObrotLasera = MathHelper.Pi;
                        Vector2 Normalna = new Vector2(); ;
                        if (!LaserNaprowadzany[i])
                        {
                            float zmianaX;
                            float zmianaY;
                            zmianaX = SciezkaX.Evaluate(AktT) - Pozycja.X;
                            zmianaY = SciezkaY.Evaluate(AktT) - Pozycja.Y;
                            Normalna = new Vector2(zmianaX, zmianaY);
                            Normalna.Normalize();
                        }

                        else 
                        {
                            Normalna = GlobalAcc.StatekGracza.Pozycja - Pozycja;
                            Normalna.Normalize();
                            ObrotLasera = MathHelper.PiOver2 + (float)Math.Atan2(Normalna.Y, Normalna.X);
                        }

                        LaserWroga nowy = new LaserWroga(Pozycja + PozycjaLasera[i], ref BronLaser[i].Pierwowzor.Obrazek, ObrotLasera, BronLaser[i].Pierwowzor.Sila,
                            Normalna, BronLaser[i].Pierwowzor.V, BronLaser[i].Pierwowzor.KolorLaseru);
                        GlobalAcc.ListaObiektow2.Add(nowy);
                    }
            }///Laser Weapon
             ///
            if(rakieta)
            for (int i = 0; i < BronRakieta.Length; i++)
            {
                BronRakieta[i].Pracuj();
                if (RakietyStrzelaj[i])
                if (BronRakieta[i].Strzel())
                {
                    float ObrotRakiety = MathHelper.Pi ;
                    if (RakietyNaprowadzane[i])
                    {
                        Vector2 dif = GlobalAcc.StatekGracza.Pozycja - Pozycja;
                        ObrotRakiety = MathHelper.PiOver2 + (float)Math.Atan2(dif.Y, dif.X);
                    }
                    RakietaWroga nowa = new RakietaWroga(BronRakieta[i].Pierwowzor.Kopia(Pozycja + PozycjaRakiet[i], BronRakieta[i].Pierwowzor.V, ObrotRakiety));
                    Random rand = new Random();
                    if (rand.Next(0, 2) == 1) nowa.Niestabilnosc = -nowa.Niestabilnosc;
                    GlobalAcc.ListaObiektow1.Add(nowa);
                }
            }
        }


        public override StatekWroga Kopia()
        {
            BossCzacha ret = new BossCzacha();
            return ret;
        }
    } 

}