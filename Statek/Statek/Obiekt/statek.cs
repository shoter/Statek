using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Damian.MWGK.Obiekt.Bron;
using Damian.MWGK.Global.Test;
using Damian.MWGK.Particle;
using Damian.MWGK.Particle.Storage;
using Damian.MWGK.Particle.System;
using Damian.MWGK.Sprite;
using Damian.MWGK.Global.Graphics;

namespace Damian.MWGK.Obiekt.Statek
{
    class StatekKosmiczny : Obiekt
    {
        public float AktTarcza, MaxTarcza, RegTarcza;
        public float AktKadlub, MaxKadlub;

        public Curve SciezkaX = new Curve(), SciezkaY = new Curve() ; //mozliwosc zdefiniowania sciezek dla obiektu
        public float AktT,maxT;
        public float TPlus = 1.0f; //o ile zwieksza sie AktT
        protected bool UzyjSciezek = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pozycja"></param>
        /// <param name="obrazek"></param>
        /// <param name="obrot"></param>
        /// <param name="tarcza">maxymalna ilosc tarczy jaka objekt posiada</param>
        /// <param name="regtarcza">regeneracja tarczy na 1 klatke</param>
        /// <param name="kadlub">maxymalna ilosc kadluba ktora posiada obiekt</param>
        /// <param name="bronl">bron laserowa obiektu</param>
        public StatekKosmiczny(Vector2 pozycja, ref Texture2D obrazek, float obrot,
                    float tarcza, float regtarcza, float kadlub)
            : base(pozycja,ref obrazek, obrot)
        {
            AktTarcza = tarcza; MaxTarcza = tarcza;
            RegTarcza = regtarcza;
            AktKadlub = kadlub; MaxKadlub = kadlub;
            UzyjSciezek = false;

        }
        public StatekKosmiczny(Vector2 pozycja,ref Texture2D obrazek,float obrot)
            :base(pozycja,ref obrazek,obrot)
        
        {}

        /// <summary>
        /// WSZYSTKIE ARGUMENTY MAJA MIEC TAKA SAMA DLUGOSC TABLICY!!!
        /// </summary>
        /// <param name="sciezkax"></param>
        /// <param name="sciezkay"></param>
        public virtual void RuchSciezka(CurveKey []sciezkax,CurveKey[] sciezkay,int t)
        {
            for (int i = 0; i < sciezkax.Length; i++)
            {
                SciezkaX.Keys.Add(sciezkax[i]);
                SciezkaY.Keys.Add(sciezkay[i]);
            }
            AktT = 0;
            maxT = t;
            UzyjSciezek = true;

            SciezkaX.ComputeTangents(CurveTangent.Smooth);
            SciezkaY.ComputeTangents(CurveTangent.Smooth);
        }

        public void RuchSciezka(Curve sciezkax, Curve sciezkay, int t)
        {
            SciezkaX = sciezkax;
            SciezkaY = sciezkay;
            AktT = 0;
            maxT = t;
            UzyjSciezek = true;

        }

        public override void Pracuj()
        {
            if (Zniszczony) return;
            if (UzyjSciezek == false)
                base.Pracuj();
            else
            {
                float zmianaX;
                float zmianaY;
                zmianaX = SciezkaX.Evaluate(AktT) - Pozycja.X;
                zmianaY = SciezkaY.Evaluate(AktT) - Pozycja.Y;
                //Normalna.X = zmianaX;
                //Normalna.Y = zmianaY;
                Obrot = MathHelper.PiOver2 + (float)Math.Atan2(zmianaY, zmianaX);

               Pozycja.X +=  zmianaX;
               Pozycja.Y +=  zmianaY;
               AktT += TPlus; 
            }
            if (AktTarcza < MaxTarcza)
            {
                AktTarcza += RegTarcza;
                if (AktTarcza > MaxTarcza) AktTarcza = MaxTarcza;
            }


        }


        public override void ReakcjaKolizja(float sila, Vector2 gdzie)
        {
            if (Zniszczony) return;
            if (AktTarcza > 0)
            {
                AktTarcza -= sila;
                sila = 0f;

                float Alpha = (AktTarcza / MaxTarcza) * 255;
                Color start1 = new Color(100, 255, 255,(int) Alpha);
                Color koniec =  new Color(100, 255, 255, 0);

                float odl = Vector2.Distance(gdzie,Pozycja);
                Vector2 dif = gdzie - Pozycja;
                float obr = MathHelper.PiOver2 + (float)Math.Atan2(dif.Y, dif.X);

                
                ParticleCirle Tarcza = new ParticleCirle((int)(Pozycja.X + Normalna.X * V), (int)(Pozycja.Y + Normalna.Y * V), 2, 4, 2, 4, start1, start1, koniec, Color.Transparent,
                MaxR / 2 + 10f, MaxR / 2 + 10f, MaxR / 2 + 10f, MaxR / 2 + 10f, -MathHelper.PiOver4 + obr,MathHelper.PiOver4 + obr, 12, 15, 1, Rodzaj_Ruchu.RuchZaden, 30);
                Tarcza.Przyczep(this); //tarcza pod¹¿a za statkiem

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
                }
            }
        }


        public override void Rysuj(ref SpriteBatch SB)
        {
            if (!Zniszczony)
                base.Rysuj(ref SB);
            
        }

    
        
       
        
    }
}