using Damian.MWGK.Obiekt.Bron;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Damian.MWGK.Particle;
using Damian.MWGK.Particle.Storage;
using System;
using Damian.MWGK.Global.Test;
using Damian.MWGK.Global.Graphics;
namespace Damian.MWGK.Obiekt.Bron
{
    class RakietaWroga : Rakieta
    {
        public RakietaWroga(Vector2 pozycja, ref Texture2D obrazek, float obrot, float sila,
                        ParticleGun silnik, ParticleGun dym, float v, int t, float niestabilnosc)
            : base(pozycja,ref obrazek,obrot,sila,silnik,dym,v,t,niestabilnosc)
        { }

        public RakietaWroga(Rakieta Inny)
            :base(Inny.Pozycja,ref Inny.Obrazek,Inny.Obrot,Inny.Sila,Inny.SilnikPlomien,Inny.SilnikDym,Inny.V,Inny.KT,Inny.Niestabilnosc)
        {        }

        public override void Pracuj()
        {
            if (!Zniszczony)
            {
                Random rand;
                rand = new Random();
                Vector2 srodek = Pozycja + rozmiary / 2;
                Obrot += (float)(rand.Next(0, 100)) / 100f * Niestabilnosc;
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

                    if (Kolizja(GlobalAcc.StatekGracza))
                    {
                        float test1 = OdlegloscDo(GlobalAcc.StatekGracza);
                        Vector2 dif = Pozycja - GlobalAcc.StatekGracza.Pozycja;
                        float obrot_wybuchu = MathHelper.PiOver2 + (float)Math.Atan2(dif.Y, dif.X);
                        Color koncowy = new Color(255, 0, 0, 30);
                        Wybuch = new ParticleCirle((int)Pozycja.X, (int)Pozycja.Y, 2, 4, 2, 4, Color.Yellow, Color.Orange, koncowy, koncowy, 0f, 0f, 54f, 55f, obrot_wybuchu - MathHelper.PiOver2, obrot_wybuchu + MathHelper.PiOver2,
                            13, 15, 1,Rodzaj_Ruchu.RuchPrzyspieszony, 100);
                        Zniszczony = true;
                        GlobalAcc.StatekGracza.ReakcjaKolizja(Sila, Pozycja);
                    }
            }
            else
            {
                Wybuch.Pracuj(true);
                if (SekwencjaWybuchu < Grafika.Eksplozja1.IloscX * 2 - 1)
                SekwencjaWybuchu++;
                
            }



        }
    }
}