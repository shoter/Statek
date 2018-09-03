using Damian.MWGK.Obiekt.Statek;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Damian.MWGK.Obiekt;
using System;
using System.Collections.Generic;

namespace Damian.MWGK.Fala
{
    //klase tutaj sa FULL publiczne,bo sa bardziej duzym kontenerem na to co bedzie dzialo sie potem aniżeli na to co robie teraz
    //a co robie teraz?Nobody knows
    class StatekInformacja
    {
        public StatekWroga Statek;
        public int StartT, EndT;
        public int[] LaserStart, LaserEnd;
        public int[] RakietyStart, RakietyEnd;
        public int CzestRakiet, CzestLaser;
        public float AccelT; //ile czasu T dodaje sie na sekunde
        public bool UzywaLaser, UzywaRakieta;
        public StatekInformacja(StatekWroga statek,int startt,
           int[] laserstart,int[] laserend,int[] rakietystart,int []rakietyend,
            int czestrakiet,int czestlaser,float accelt,bool uzywalaser,bool uzywarakieta)
        {
            Statek = statek;
            StartT = startt;
            LaserStart = laserstart;
            LaserEnd = laserend;
            RakietyStart = rakietystart;
            RakietyEnd = rakietyend;
            CzestRakiet = czestrakiet;
            CzestLaser = czestlaser;
            AccelT = accelt;
            EndT = 0; //FalaInformacyjna sie tym zajmie
            UzywaLaser = uzywalaser;
            UzywaRakieta = uzywarakieta;
        }
    }

  

    class FalaInformacyjna
    {
        public Curve DrogaX, DrogaY;
        public int T; //czas trwania sciezek
        public List<StatekInformacja> Statki;
        public FalaInformacyjna(Curve X, Curve Y, int t, List<StatekInformacja> statki)
        {
            DrogaX = X;
            DrogaY = Y;
            Statki = statki;
            T = t;
        }

        public FalaInformacyjna(List<StatekInformacja> statki)
        {
            Statki = statki;
            DrogaX = null;
            DrogaY = null;
            T = 0;
        }
        public FalaInformacyjna(Curve X, Curve Y, int t,int[] laserstart,int[] laserend,int[] rakietystart,int []rakietyend,List<StatekInformacja> statki)
        {
            DrogaX = X;
            DrogaY = Y;
            Statki = statki;
            T = t;
            for (int i = 0; i < Statki.Count; i++)
            {
                Statki[i].EndT = t;
                Statki[i].Statek.Pozycja = new Vector2(X.Keys[0].Value, Y.Keys[0].Value);
                Statki[i].LaserStart = laserstart;
                Statki[i].LaserEnd = laserend;
                Statki[i].RakietyStart = rakietystart;
                Statki[i].RakietyEnd = rakietyend;
            }
        }

        public FalaInformacyjna StworzFale(Curve X, Curve Y, int t,int[] laserstart,int[] laserend,int[] rakietystart,int []rakietyend)
        {
            for (int i = 0; i < Statki.Count; i++)
            {
                Statki[i].EndT = t;
                Statki[i].Statek.Pozycja = new Vector2(X.Keys[0].Value, Y.Keys[0].Value);
                Statki[i].LaserStart = laserstart;
                Statki[i].LaserEnd = laserend;
                Statki[i].RakietyStart = rakietystart;
                Statki[i].RakietyEnd = rakietyend;
            }
            return new FalaInformacyjna(X, Y, t, Statki);
        }

        public FalaInformacyjna(Curve X, Curve Y, int t)
        {
            DrogaX = X;
            DrogaY = Y;
            Statki = new List<StatekInformacja>();
            T = t;
        }

        /// <summary>
        /// Ustal sciezke przed dodaniem informacji o jakimkolwiek statku
        /// </summary>
        /// <param name="SI"></param>
        public void Dodaj(StatekInformacja SI)
        {
            SI.EndT = T;
            SI.Statek.Pozycja = new Vector2(DrogaX.Keys[0].Value, DrogaY.Keys[0].Value);
            Statki.Add(SI);
        }

    }
}