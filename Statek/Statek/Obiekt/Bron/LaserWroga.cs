using Damian.MWGK.Obiekt;
using Damian.MWGK.Obiekt.Bron;
using Damian.MWGK.Global.Test;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

using Damian.MWGK.Particle.System;
using Damian.MWGK.Particle.Storage;

namespace Damian.MWGK.Obiekt.Bron
{
    class LaserWroga : Laser
    {
        public LaserWroga(Vector2 pozycja, ref Texture2D obrazek, float obrot, float sila, Vector2 normalna, float v,Color kollaser)
            : base(pozycja,ref obrazek, obrot,sila,normalna,v,kollaser)
        {  }

        public override void Pracuj()
        {
            if (Ruch != Rodzaj_Ruchu.RuchZaden || !Zniszczony)
            {
                Pozycja += Normalna * V;
                V += A;
                Odleglosc -= V;

            }

            if (!Zniszczony)
                if (Kolizja(GlobalAcc.StatekGracza))
                    {
                        Zniszczony = true;
                        GlobalAcc.StatekGracza.ReakcjaKolizja(Sila, Pozycja);
                        Vector2 dif = Pozycja - GlobalAcc.StatekGracza.Pozycja;
                        float obrot_wybuchu = MathHelper.PiOver2 + (float)Math.Atan2(dif.Y, dif.X);

                        Color koncowy = KolorLaseru;
                        koncowy.A = 0;

                        ParticleSystemComponent PSC = new ParticleSystemComponent(
                        true, true, new ParticleCirle((int)Pozycja.X, (int)Pozycja.Y, 2, 3, 2, 3, KolorLaseru, KolorLaseru, koncowy, koncowy, 0, 0, 10, 10,
                            0f, MathHelper.TwoPi, 6, 6, 1, Rodzaj_Ruchu.RuchPrzyspieszony, 360));

                        ParticleSystem.dodaj(PSC);
                    }


        }

    }
}