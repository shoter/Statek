#define DEBUG
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Damian.MWGK.Obiekt; //dla rodzaju ruchu
using Damian.MWGK.Particle;
using Damian.MWGK.Particle.Storage;
using Damian.MWGK.Global.Test;

namespace Damian.MWGK.Particle.Storage
{

    enum Rodzaj_Dziala : byte
    {
        Cosinusowe = 1, //czyli krozysta z funkcji trygonometrycznych do strzelania
        Rownomierne = 2, //korzysta z randa ale wszystko ulozone jest mniej wiecej rownomiernie
        Losowo = 3
    };//wszystko jest losowo
    class ParticleGun : ParticleStorage
    {
        //Właściwosci cząsteczek wystrzeliwanych
        public int X, Y;
        protected int MinSzer, MaxSzer;
        protected int MinWys, MaxWys;
        protected Color MinSKolor, MaxSKolor;
        protected Color MinKKolor, MaxKKolor;
        protected float MinObrot, MaxObrot;
        protected float MinR,MaxR;
        protected int T, KT; //ten typ storage'a posiada czas!
        protected int MinT, MaxT;
        protected Rodzaj_Ruchu Ruch; //ruch poczatkowy jakim beda poruszaly sie obiekty
        protected int Ilosc; //tworzonych czasteczek na klatke
        protected Rodzaj_Dziala Ulozenie;
        public float Obrot;

   /// <summary>
   /// 
   /// </summary>
   /// <param name="x"></param>
   /// <param name="y"></param>
   /// <param name="minszer"></param>
   /// <param name="maxszer"></param>
   /// <param name="minwys"></param>
   /// <param name="maxwys"></param>
   /// <param name="minskolor"></param>
   /// <param name="maxskolor"></param>
   /// <param name="minkkolor"></param>
   /// <param name="maxkkolor"></param>
   /// <param name="minobrot">Minimalny kat wystralu czasteczek</param>
   /// <param name="maxobrot">Max kat pod jakim beda wystrzeliwane czasteczki</param>
   /// <param name="minr">minimalna odleglosc tworzenia czasteczki od celu</param>
   /// <param name="maxr">Maxymalna odleglosc tworzenia czasteczki od celu</param>
   /// <param name="mint">minimalny czas zycia czasteczki w klatkach</param>
   /// <param name="maxt">maxymalny czas zycia czasteczki w klatkac</param>
   /// <param name="t">czas zycia dziala czasteczkowe(0 = nieskonczonosc)</param>
   /// <param name="ruch">Jakim ruchem poruszaja sie czasteczki</param>
   /// <param name="ilosc">ilosc czasteczek tworzonych na klatke</param>
   /// <param name="ulozenie">Ulozenie tworzonych czasteczek wgledem dziala</param>
        public ParticleGun(int x, int y, int minszer, int maxszer, int minwys, int maxwys
                           , Color minskolor, Color maxskolor, Color minkkolor, Color maxkkolor, float minobrot, float maxobrot,float minr,float maxr,
                            int mint,int maxt,int t, Rodzaj_Ruchu ruch,int ilosc,Rodzaj_Dziala ulozenie)
        {
            X = x; Y = y;
            MinSzer = minszer; MaxSzer = maxszer;
            MinWys = minwys;MaxWys = maxwys;
            MinSKolor = minskolor;MaxSKolor = maxskolor;
            MinKKolor = minkkolor; MaxKKolor = maxkkolor;
            MinObrot = minobrot; MaxObrot = maxobrot;
            MinT = mint; MaxT = maxt;
            MinR = minr;MaxR = maxr;
            T = 0;
            KT = t;
            if (KT < 0) KT = 1;
            Ruch = ruch;
            Ilosc = ilosc;
            Ulozenie = ulozenie;
            Obrot = 0f;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TworzCzasteczki">Najlepiej daj true</param>
        public override void Pracuj(bool TworzCzasteczki)
        {
            

            foreach (Particle X in Obiekty)
            {
                X.Pracuj();

            }
            for (int i = 0; i < Obiekty.Count; i++)
            {
                if (Obiekty[i].Koniec)
                {
                    Obiekty.RemoveAt(i);
#if DEBUG
                    GlobalAcc.Czasteczki--;
#endif
                }
            }

            if (T == KT && KT != 0)
            {
                Koniec = true;
                
                return;
            }
            T++;
            //najpierw stworzmy nowe czasteczki
            if (TworzCzasteczki)
            for (int i = 0; i < Ilosc; i++)
            {
                Obiekty.Add(StworzCzasteczke(i));
            }
            //teraz operujemy na czasteczkach
            

        }




        private DynamicParticle StworzCzasteczke(int i) //i pomocnicza
        {

            DynamicParticle czasteczka;
            Random rand = new Random();
            czasteczka = new DynamicParticle(X, Y, rand.Next(MinSzer, MaxSzer), rand.Next(MinWys, MaxWys),
                Color.Lerp(MinSKolor, MaxSKolor, ((float)rand.Next(0, 100)) / 100f),
                Color.Lerp(MinKKolor, MaxKKolor, ((float)rand.Next(0, 100)) / 100f), rand.Next(MinT, MaxT));
            czasteczka.Vmax = 100f;

            
            float helpX=0f, helpY=0f;
            float helpobrot = Obrot - (float)Math.PI;


            switch (Ulozenie)
            {
                case Rodzaj_Dziala.Cosinusowe:
                    {
                        float ile = (float)(i + 1) / (float)Ilosc * (float)Math.PI;
                        float obrot = (float)Math.Sin(ile) * MathHelper.Lerp(MinObrot,MaxObrot,(float)(rand.Next(0,100)/100f)) + helpobrot;
                        helpX = (float)Math.Sin(obrot) * MathHelper.Lerp(MinR,MaxR,(float)rand.Next(0,100)/ 100f);
                        helpY -= (float)Math.Cos(obrot) * MathHelper.Lerp(MinR, MaxR, (float)rand.Next(0, 100) / 100f);
                    }
                    break;
                case Rodzaj_Dziala.Losowo:
                    {
                        float ile = (float)rand.Next(0,100)/100f * (float)Math.PI;
                        float obrot = (float)Math.Sin(ile) * MathHelper.Lerp(MinObrot, MaxObrot, (float)(rand.Next(0, 100) / 100f)) + helpobrot;
                        helpX = (float)Math.Sin(obrot) * MathHelper.Lerp(MinR, MaxR, (float)rand.Next(0, 100) / 100f);
                        helpY -= (float)Math.Cos(obrot) * MathHelper.Lerp(MinR, MaxR, (float)rand.Next(0, 100) / 100f);
                    }
                    break;
                case Rodzaj_Dziala.Rownomierne:
                    {
                        float ile = (float)(i + 1) / (float)Ilosc;
                        float obrot = ile * MathHelper.Lerp(MinObrot, MaxObrot, (float)(rand.Next(0, 100) / 100f)) + helpobrot;
                        helpX = (float)Math.Sin(obrot) * MathHelper.Lerp(MinR, MaxR, (float)rand.Next(0, 100) / 100f);
                        helpY -= (float)Math.Cos(obrot) * MathHelper.Lerp(MinR, MaxR, (float)rand.Next(0, 100) / 100f);
                    }
                    break;
            }

            

            Vector2 dokad = new Vector2(
                helpX + (float)X,
                helpY + (float)Y);                


            switch (Ruch)
            {
                case Rodzaj_Ruchu.RuchJednostajny :
                    czasteczka.RuchJednostajny(dokad);
                    break;
                case Rodzaj_Ruchu.RuchPrzyspieszony :
                    czasteczka.RuchJednostajny(dokad);
                    break;
            }
            return czasteczka;
        }
        public ParticleGun Kopia()
        {
            ParticleGun ret = new ParticleGun(X, Y, MinSzer, MaxSzer, MinWys, MaxWys, MinSKolor, MaxSKolor, MinKKolor, MaxKKolor,
                MinObrot, MaxObrot, MinR, MaxR, MinT, MaxT, T, Ruch, Ilosc, Ulozenie);
            return ret;
        }
    }

     
}