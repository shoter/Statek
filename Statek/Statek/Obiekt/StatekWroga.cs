using Damian.MWGK.Obiekt.Statek;
using Damian.MWGK.Obiekt.Statek.Gracz;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Damian.MWGK.Global.Test;
using Damian.MWGK.Obiekt.Bron;
using System;
using Damian.MWGK.Global.Zbiory;
using Damian.MWGK.Obiekt.Powers;
using Damian.MWGK.Particle.Storage;
namespace Damian.MWGK.Obiekt.Statek
{
    class StatekWroga  : StatekKosmiczny
    {
        public DzialoLaserowe BronLaser { protected set; get; }
        public DzialoRakietowe BronRakieta { protected set; get; }
        public bool LaserNaprowadzany { protected set; get; }
        public bool RakietyNaprowadzane { protected set; get; }
        public int Punkty; //ile punktow za przeciwnika,easy
        ParticleGun Silnik;

        private int IDRakiety,IDLasera; //do kopiowania

        protected PowerUp[] ListaBonusow;
        protected int Szansa; //0-1000

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pozycja"></param>
        /// <param name="obrazek"></param>
        /// <param name="obrot"></param>
        /// <param name="tarcza"></param>
        /// <param name="regtarcza"></param>
        /// <param name="kadlub"></param>
        /// <param name="IDR">ID Rakiet jakie dostanie statek</param>
        /// <param name="IDL">ID lasera jakie dostanie statek</param>
        /// <param name="rakietynaprowadzane"></param>
        /// <param name="lasernaprowadzany"></param>
        public StatekWroga(Vector2 pozycja, ref Texture2D obrazek, float obrot,
                    float tarcza, float regtarcza, float kadlub, int IDR, int IDL, bool lasernaprowadzany, bool rakietynaprowadzane, int pkt, PowerUp[] Bonusy, int szansa, ParticleGun silnik)
        :base(pozycja,ref obrazek,obrot,tarcza,regtarcza,kadlub)
        {
            
           
            if (IDR != -1)
            {
            BronLaser = Wyposazenie.Lasery[IDL].Kopia();
            BronRakieta = Wyposazenie.Rakiety[IDR].Kopia();
            }
            else
            {
                BronLaser = null;
                BronRakieta = null;
            }
            IDLasera = IDL;
            IDRakiety = IDR;

            LaserNaprowadzany = lasernaprowadzany;
            RakietyNaprowadzane = rakietynaprowadzane;

            Punkty = pkt;

            ListaBonusow = Bonusy;
            Szansa = szansa;
            Silnik = silnik;
        }

        public virtual void Pracuj(bool laser, bool rakieta)
        {
            
            base.Pracuj();
            if (Silnik != null)
            {
                Silnik.X = (int)Pozycja.X;
                Silnik.Y = (int)Pozycja.Y;
                Silnik.Obrot = Obrot;
                Silnik.Pracuj(!Zniszczony);
            }
            if (Zniszczony) return;
            BronLaser.Pracuj();
            BronRakieta.Pracuj();

            if (laser && BronLaser != null)
                if (BronLaser.Strzel())
                {
                    float ObrotLasera = Obrot;
                    Vector2 Normalna = new Vector2(); ;
                    if(!LaserNaprowadzany)
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

                    LaserWroga nowy = new LaserWroga(Pozycja, ref BronLaser.Pierwowzor.Obrazek, ObrotLasera, BronLaser.Pierwowzor.Sila,
                        Normalna, BronLaser.Pierwowzor.V,BronLaser.Pierwowzor.KolorLaseru);
                    GlobalAcc.ListaObiektow2.Add(nowy);
                }
            if (rakieta && BronRakieta != null)
                if (BronRakieta.Strzel())
                {
                    float ObrotRakiety = Obrot;
                    if (RakietyNaprowadzane)
                    {
                        Vector2 dif = GlobalAcc.StatekGracza.Pozycja - Pozycja;
                        ObrotRakiety = MathHelper.PiOver2 + (float)Math.Atan2(dif.Y, dif.X);
                    }
                    RakietaWroga nowa = new RakietaWroga(BronRakieta.Pierwowzor.Kopia(Pozycja, BronRakieta.Pierwowzor.V, ObrotRakiety));
                    Random rand = new Random();
                    if (rand.Next(0, 2) == 1) nowa.Niestabilnosc = -nowa.Niestabilnosc;
                    GlobalAcc.ListaObiektow1.Add(nowa);
                }

           
        }

        public override void ReakcjaKolizja(float sila, Vector2 gdzie)
        {
            bool Caly = true;
            if (AktKadlub < 0f) Caly = false;
            base.ReakcjaKolizja(sila, gdzie);
            if (Caly && AktKadlub < 0f) //zostal zniszczony w trakcie kolizji
            {
                GlobalAcc.StatekGracza.IloscPkt += Punkty;
                if (GlobalAcc.StatekGracza.DoublePoints.active) GlobalAcc.StatekGracza.IloscPkt += Punkty;
                if (ListaBonusow != null)
                {
                    Random rand = new Random();
                    if (rand.Next(0, 1000) <= Szansa)
                    {
                        PowerUp PU = ListaBonusow[rand.Next(0, ListaBonusow.Length)].Kopia(Pozycja, 0f);
                        PU.RuchJednostajny(new Vector2(Pozycja.X, 900f), 4f, true);
                        PU.zObrot = ((float)rand.Next(25, 100) / 360.0f);
                        GlobalAcc.ListaBonusow.Add(PU);
                    }
                }
            }
        }


        public virtual StatekWroga Kopia()
        {
            StatekWroga ret = new StatekWroga(Pozycja, ref Obrazek, Obrot, MaxTarcza, RegTarcza, MaxKadlub,IDRakiety,IDLasera,LaserNaprowadzany,RakietyNaprowadzane,100,ListaBonusow,Szansa,Silnik.Kopia());
            ret.SciezkaX = this.SciezkaX;
            ret.SciezkaY = this.SciezkaY;
            ret.UzyjSciezek = this.UzyjSciezek;

            return ret;
        }

        public override void Rysuj(ref SpriteBatch SB)
        {
            if(Silnik != null)
            Silnik.Rysuj(ref SB);
            base.Rysuj(ref SB);
        }

        public override void Czysc()
        {
            Silnik.Czysc(); ;
        }
    }
}