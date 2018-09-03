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
using Damian.MWGK.Global.Graphics;
using Damian.MWGK.Sprite;
using Damian.MWGK.Particle;
using Damian.MWGK.Particle.System;
namespace Damian.MWGK.Obiekt.Statek
{
    class BossCzacha : Boss
    {
        public ParticleGun LeweOko;
        public ParticleGun PraweOko;
        public BossCzacha()
            : base(Vector2.Zero,ref Grafika.Bossowie[0], 0f, 30f, 1.5f, 100f, null, null, null, null, null, null, 10000)
        {
            PozycjaRakiet = new Vector2[2];
            PozycjaRakiet[0] = new Vector2(20, 93);
            PozycjaRakiet[1] = new Vector2(134, 93);
            PozycjaLasera = new Vector2[1];
            PozycjaLasera[0] = new Vector2(80, 84);

            BronRakieta = new DzialoRakietowe[2];
            BronRakieta[0] = Wyposazenie.Rakiety[1].Kopia();
            BronRakieta[0].Pierwowzor.V = 5f;
            BronRakieta[1] = Wyposazenie.Rakiety[1].Kopia();
            BronLaser = new DzialoLaserowe[1];
            BronLaser[0] = Wyposazenie.Lasery[3].Kopia();
            BronLaser[0].Czestotliwosc = 5;
            LaserNaprowadzany = new bool[1];
            LaserNaprowadzany[0] = true;
            RakietyNaprowadzane = new bool[2];
            RakietyNaprowadzane[0] = false;
            RakietyNaprowadzane[1] = false;

            LaserStrzelaj = new bool[1];
            RakietyStrzelaj = new bool[2];
            RakietyStrzelaj[0] = true;
            RakietyStrzelaj[1] = true;

            if (PozycjaLasera != null)
                for (int i = 0; i < PozycjaLasera.Length; i++)
                {
                    PozycjaLasera[i].X -= rozmiary.X / 2;
                    PozycjaLasera[i].Y -= rozmiary.Y / 2;
                }
            if (PozycjaRakiet != null)
                for (int i = 0; i < PozycjaRakiet.Length; i++)
                {
                    PozycjaRakiet[i].X -= rozmiary.X / 2;
                    PozycjaRakiet[i].Y -= rozmiary.Y / 2;
                }

            LeweOko = new ParticleGun(0, 0, 2, 4, 2, 4, Color.Orange, Color.Yellow, Color.Red, Color.Red, 0f, MathHelper.TwoPi, 0, 40, 5, 15, 0, Rodzaj_Ruchu.RuchJednostajny, 30, Rodzaj_Dziala.Losowo);
            PraweOko = LeweOko.Kopia();
        }

        public override void Pracuj(bool laser,bool rakiety)
        {
            if (AktKadlub < MaxKadlub / 2)
                LaserStrzelaj[0] = true;
            base.Pracuj(laser,rakiety);
            Obrot = 0f;
            AktT %= (maxT);

            LeweOko.X = 64 - (int)rozmiary.X / 2 + (int)Pozycja.X;
            PraweOko.X = 94 - (int)rozmiary.X / 2 + (int)Pozycja.X;
            LeweOko.Y = 66 - (int)rozmiary.Y / 2 + (int)Pozycja.Y;
            PraweOko.Y = LeweOko.Y;
            LeweOko.Pracuj(!Zniszczony);
            PraweOko.Pracuj(!Zniszczony);
        }


        public override void ReakcjaKolizja(float sila, Vector2 gdzie)
        {
                if (Zniszczony) return;
                if (AktTarcza > 0)
                {
                    AktTarcza -= sila;
                    sila = 0f;

                    float Alpha = (AktTarcza / MaxTarcza) * 255;
                    Color start1 = new Color(100, 255, 255, (int)Alpha);
                    Color koniec = new Color(100, 255, 255, 0);

                    float odl = Vector2.Distance(gdzie, Pozycja);
                    Vector2 dif = gdzie - Pozycja;
                    float obr = MathHelper.PiOver2 + (float)Math.Atan2(dif.Y, dif.X);


                    ParticleCirle Tarcza = new ParticleCirle((int)(Pozycja.X + Normalna.X * V), (int)(Pozycja.Y + Normalna.Y * V), 2, 4, 2, 4, start1, start1, koniec, Color.Transparent,
                    MaxR / 2 + 15f, MaxR / 2 + 15f, MaxR / 2 + 10f, MaxR / 2 + 10f, -MathHelper.PiOver4 + obr, MathHelper.PiOver4 + obr, 12, 15, 1, Rodzaj_Ruchu.RuchZaden, 120);
                    Tarcza.Przyczep(this); //tarcza podąża za statkiem

                    ParticleSystemComponent PSC = new ParticleSystemComponent(true, true, Tarcza);
                    ParticleSystem.dodaj(PSC);

                    if (AktTarcza < 0f)
                    {
                        sila = -AktTarcza;
                        AktTarcza = 0f;
                    }
                }
                if (sila > 0f)
                {
                    AktKadlub -= sila;

                    if (AktKadlub < 0f)
                    {
                        Random rand = new Random();
                        Zniszczony = true;
                        SpriteAnimated[] Wybuchy = { new SpriteAnimated((int)Pozycja.X, (int)Pozycja.Y, Grafika.Eksplozja3, 25) };
                        foreach (SpriteAnimated X in Wybuchy)
                            GlobalAcc.ListaAnimacji1.Add(X);
                        GlobalAcc.StatekGracza.IloscPkt += Punkty;
                        if (GlobalAcc.StatekGracza.DoublePoints.active) GlobalAcc.StatekGracza.IloscPkt += Punkty;
                    }
                }

            
        }


        public override void Rysuj(ref SpriteBatch SB)
        {
            if (!Zniszczony)
            {
                LeweOko.Rysuj(ref SB);
                PraweOko.Rysuj(ref SB);
                SB.Draw(Obrazek, Pozycja, null, Color.White, Obrot, Srodek, Vector2.One, SpriteEffects.None, 0);
            }
        }


    }

}