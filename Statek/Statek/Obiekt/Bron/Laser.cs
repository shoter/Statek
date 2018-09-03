using Damian.MWGK.Obiekt;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Damian.MWGK.Global.Test;
using System;
using Damian.MWGK.Particle.Storage;
using Damian.MWGK.Particle.System;
namespace Damian.MWGK.Obiekt.Bron
{
    class Laser : Obiekt
    {
        public float Sila { protected set; get; }
        public Color KolorLaseru { protected set; get; }

        public Laser(Vector2 pozycja, ref Texture2D obrazek, float obrot, float sila, Vector2 normalna, float v,Color kollaser)
            : base(pozycja,ref obrazek, obrot)
        {
            Sila = sila;
            Normalna = normalna;
            V = v;
            Ruch = Rodzaj_Ruchu.RuchJednostajny;
            KolorLaseru = kollaser;
        }

        public Laser(Vector2 pozycja, string obrazek, ContentManager CM, float obrot, float sila,Vector2 normalna,float v,Color kollaser)
            : base(pozycja, obrazek, CM, obrot)
        {
            Sila = sila;
            RuchJednostajny(normalna, v);
            KolorLaseru = kollaser;
        }

        private void RuchJednostajny(Vector2 normalna,float v)
        {
            Normalna = normalna;
            V = v;
            Ruch = Rodzaj_Ruchu.RuchJednostajny;
            BrakStop = true;
        }

        public override void Pracuj()
        {
            if (Ruch != Rodzaj_Ruchu.RuchZaden || !Zniszczony)
            {
                Pozycja += Normalna * V;
                V += A;
                Odleglosc -= V;

            }

            if(!Zniszczony)
            foreach (Obiekt temp in GlobalAcc.ListaKolizji)
                if (Kolizja(temp))
                {
                    Zniszczony = true;
                    temp.ReakcjaKolizja(Sila, Pozycja);

                     Vector2 dif = Pozycja - temp.Pozycja;
                        float obrot_wybuchu = MathHelper.PiOver2 + (float)Math.Atan2(dif.Y, dif.X);

                        Color koncowy = KolorLaseru;
                        koncowy.A = 0;

                    ParticleSystemComponent PSC = new ParticleSystemComponent(
                        true, true, new ParticleCirle((int)Pozycja.X, (int)Pozycja.Y, 2, 3, 2, 3, KolorLaseru, KolorLaseru, koncowy, koncowy, 0, 0, 10, 10,
                            0f,MathHelper.TwoPi,6,6,1,Rodzaj_Ruchu.RuchPrzyspieszony,360));

                    ParticleSystem.dodaj(PSC);
                    break;
                }


        }


    }

}