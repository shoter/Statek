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

    class ParticleCirle : ParticleStorage
    {
        //Właściwosci cząsteczek wystrzeliwanych
        protected int X, Y;
        protected int MinSzer, MaxSzer;
        protected int MinWys, MaxWys;
        protected Color MinSKolor, MaxSKolor;
        protected Color MinKKolor, MaxKKolor;
        protected float MinOdR,MaxOdR,MinDoR,MaxDoR; //ramie koła
        protected float MinObrot, MaxObrot;
        protected int MinT,MaxT;
        protected int TCircle;
        protected int KT;
        protected int Ilosc;
        Rodzaj_Ruchu Ruch;

        bool Przyczepiaj = false; //czy przyczepiac czastki
        Obiekt.Obiekt PrzyczepienieKto; //do kogo przyczepiac

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
        /// <param name="minodr">minimalna odleglosc od punktu tworzenia czasteczek</param>
        /// <param name="maxodr">maxymalna odleglosc od punktu tworzenia czasteczek</param>
        /// <param name="mindor">minimalna odleglosc od punktu do ktorego czasteczki sie udadza</param>
        /// <param name="maxdor">maxymalna odleglosc od punktu do ktorego czasteczki sie udzadza</param>
        /// <param name="minobrot"></param>
        /// <param name="maxobrot"></param>
        /// <param name="mint">minimalny czas zycia czasteczek</param>
        /// <param name="maxt">maxymalny czas zycia czasteczek</param>
        /// <param name="tcircle">czas zycia particle circle</param>
        /// <param name="ruch">jakim ruchem czasteczki sie poruszaja</param>
        /// <param name="ilosc">ilosc tworzonych czasteczek na jedno t</param>
        public ParticleCirle(int x, int y, int minszer, int maxszer, int minwys, int maxwys
                           , Color minskolor, Color maxskolor, Color minkkolor, Color maxkkolor, float minodr,float maxodr,float mindor,float maxdor,float minobrot,float maxobrot,
                            int mint,int maxt,int tcircle, Rodzaj_Ruchu ruch, int ilosc)
        {
            X = x; Y = y;
            MinSzer = minszer; MaxSzer = maxszer;
            MinWys = minwys; MaxWys = maxwys;
            MinSKolor = minskolor; MaxSKolor = maxskolor;
            MinKKolor = minkkolor; MaxKKolor = maxkkolor;
            MinObrot = minobrot; MaxObrot = maxobrot;
            MinOdR = minodr; MaxOdR = maxodr;
            MinDoR = maxdor; MaxDoR = maxdor;

            Ilosc = ilosc;
            Ruch = ruch;
            MinT = mint;MaxT = maxt;
            TCircle = 0;
            KT = tcircle;
           

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


            if (TCircle == KT)
            {
                Koniec = true;
                return;
            }
            TCircle++;
            if (TworzCzasteczki)
            for (int i = 0; i < Ilosc; i++)
                Obiekty.Add(StworzCzasteczke(i));


        }


        public void Przyczep(Obiekt.Obiekt Co)
        {
            PrzyczepienieKto = Co;
            Przyczepiaj = true;
        }

        private DynamicParticle StworzCzasteczke(int i) //i pomocnicza
        {
            DynamicParticle czasteczka;
            Random rand = new Random();
            float kat = MathHelper.Lerp(MinObrot,MaxObrot,(float)(i + 1) / (float)Ilosc);
            float Sin = (float)Math.Sin(kat);
            float Cos = (float)Math.Cos(kat);
            float DoR = MathHelper.Lerp(MinDoR, MaxDoR, (float)(rand.Next(0, 100) / 100f));
            float OdR = MathHelper.Lerp(MinOdR, MaxOdR, (float)(rand.Next(0, 100) / 100f));

            czasteczka = new DynamicParticle((int)(Sin * OdR + (float)X), (int)(-Cos *OdR + (float)Y), rand.Next(MinSzer, MaxSzer), rand.Next(MinWys, MaxWys),
                Color.Lerp(MinSKolor, MaxSKolor, ((float)rand.Next(0, 1000)) / 1000f),
               Color.Lerp(MinKKolor, MaxKKolor, ((float)rand.Next(0, 1000)) / 1000f),rand.Next(MinT,MaxT));
            czasteczka.Vmax = 10f; //moze poruszac sie z ogromna predkoscia

            //wpierw oblcize dokad ma doleciec nastepna czasteczka

            Vector2 dokad = new Vector2(Sin * DoR + (float)X, -Cos * DoR + (float)Y); 
          

            switch (Ruch)
            {
                case Rodzaj_Ruchu.RuchJednostajny:
                    czasteczka.RuchJednostajny(dokad);
                    break;
                case Rodzaj_Ruchu.RuchPrzyspieszony:
                    czasteczka.RuchJednostajny(dokad);
                    break;
            }
            if (Przyczepiaj)
                czasteczka.Przyczep(PrzyczepienieKto);
            return czasteczka;
        }
    }
}