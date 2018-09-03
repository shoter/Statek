using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Statek;
using Damian.MWGK.Particle.Storage;
using Damian.MWGK.Global.Test;
using Damian.MWGK.Global.Graphics;
namespace Damian.MWGK.Obiekt.Bron
{
    class Rakieta : Obiekt
    {
        public ParticleGun SilnikPlomien { protected set; get; }
        public ParticleGun SilnikDym { protected set; get; }
        /// <summary>
        /// Pozycja poczatkowa skad lecial pocisk
        /// </summary>
        protected Vector2 Start;
        public float Sila {protected set; get; } //sila pocisku,jakas tam bedzie,nie mam pojecia jak ja wyrazic etc
        public float Niestabilnosc;
        protected int T;
        public int KT {protected set; get; }
        protected ParticleCirle Wybuch;
        protected int SekwencjaWybuchu = 0;

        public Rakieta(Vector2 pozycja,ref Texture2D obrazek, float obrot, float sila,
                        ParticleGun silnik, ParticleGun dym, float v, int t, float niestabilnosc)
            : base(pozycja,ref obrazek,0f)
        {
            Sila = sila;
            SilnikPlomien = silnik;
            SilnikDym = dym;
            Start = pozycja;
            Obrot = obrot;
            V = v;
            T = 0;
            KT = t;
            Niestabilnosc = niestabilnosc;

        }

        public override void Pracuj()
        {
            if (!Zniszczony)
            {
                Random rand;
                rand = new Random();
                Vector2 srodek = Pozycja + rozmiary / 2;
                Obrot += (float)(rand.Next(0, 1000)) / 1000f * Niestabilnosc;
                float Sin = (float)Math.Sin(Obrot);
                float Cos = (float)Math.Cos(Obrot);
                Pozycja.X += Sin * V;
                Pozycja.Y -= Cos * V;

                //pozycja silnikow
                float pozx = X;
                float pozy = (Y + rozmiary.Y / 2);


                pozx = pozx - X;
                pozy = pozy - Y;
                float tx = pozx, ty = pozy;
                pozx = tx * Cos - ty * Sin;
                pozy = tx * Sin + ty * Cos;
                pozx += X;
                pozy += Y;

                SilnikDym.X = (int)pozx; SilnikDym.Y = (int)pozy; SilnikDym.Obrot = Obrot;
                SilnikPlomien.X = (int)pozx; SilnikPlomien.Y = (int)pozy; SilnikPlomien.Obrot = Obrot;

                SilnikDym.Pracuj(true);
                SilnikPlomien.Pracuj(true);


                T++;
                if (T == KT)
                {
                    T = 0;
                    Niestabilnosc = -Niestabilnosc;
                }

                foreach (Obiekt temp in GlobalAcc.ListaKolizji)
                    if (Kolizja(temp))
                    {
                        float test1 = OdlegloscDo(temp);
                        Vector2 dif = Pozycja - temp.Pozycja;
                        float obrot_wybuchu = MathHelper.PiOver2 + (float)Math.Atan2(dif.Y, dif.X);
                        Color koncowy = new Color(255, 0, 0, 30);
                        Wybuch = new ParticleCirle((int)Pozycja.X, (int)Pozycja.Y, 2, 4, 2, 4, Color.Yellow, Color.Orange, koncowy, koncowy, 0f, 0f, 54f, 55f, obrot_wybuchu - MathHelper.PiOver2, obrot_wybuchu + MathHelper.PiOver2,
                            13, 15, 1,Rodzaj_Ruchu.RuchPrzyspieszony, 100);
                        Zniszczony = true;
                        temp.ReakcjaKolizja(Sila, Pozycja);
                        break;
                    }
            }
            else
            {
                Wybuch.Pracuj(true);
                if (SekwencjaWybuchu < Grafika.Eksplozja1.IloscX * 2 - 1)
                SekwencjaWybuchu++;
                
            }



        }

        public override void Rysuj(ref SpriteBatch SB)
        {
            if (!Zniszczony)
            {
                SilnikDym.Rysuj(ref  SB);
                SB.Draw(Obrazek, Pozycja, null, Color.White, Obrot, Srodek, Vector2.One, SpriteEffects.None, 0);
                SilnikPlomien.Rysuj(ref  SB);
            }
            else
            {
                Wybuch.Rysuj(ref SB);
                Grafika.Eksplozja1.Rysuj((int)Pozycja.X, (int)Pozycja.Y, SekwencjaWybuchu / 2, 0, ref SB);
            }
        }

        public Rakieta Kopia(Vector2 pozycja,float v,float obrot)
        {
            Rakieta ret = new Rakieta(pozycja, ref Obrazek, obrot, Sila, SilnikPlomien.Kopia(), SilnikDym.Kopia(), v, KT, Niestabilnosc);
            return ret;
        }

        public override void Czysc()
        {
            SilnikDym.Czysc();
            SilnikPlomien.Czysc();
        }
    }

}