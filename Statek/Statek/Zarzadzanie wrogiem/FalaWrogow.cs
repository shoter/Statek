using Damian.MWGK.Obiekt.Statek;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Damian.MWGK.Obiekt;
using Damian.MWGK.Global.Test;
namespace Damian.MWGK.Fala
{
    class StatekInstancja
    {
        public StatekWroga Statek;
        public int T,KT; //jego wlasne
        public int CzestRakiet, CzestLaser;
        public int[] RakietyOd, RakietyDo;
        public int[] LaseryOd, LaseryDo;
        public bool UzywaLaser, UzywaRakieta;


        public StatekInstancja(StatekInformacja SI)
        {
            Statek = SI.Statek.Kopia();
            Statek.TPlus = SI.AccelT;
            Statek.maxT = SI.EndT;
            RakietyOd = SI.RakietyStart;
            RakietyDo = SI.RakietyEnd;
            LaseryOd = SI.LaserStart;
            LaseryDo = SI.LaserEnd;
            CzestRakiet = SI.CzestRakiet;
            CzestLaser = SI.CzestLaser;
            KT = SI.EndT;
            T = SI.StartT;
            UzywaLaser = SI.UzywaLaser;
            UzywaRakieta = SI.UzywaRakieta;
        }

        public void Pracuj()
        {
            if (T >= KT) return;
            if (T < 0)
            {
                T++;
                return; //jesli jeszcze jego kolei na wejscie do fali sie nie zaczela,gdy T jest mneijsze niz 0 to statek jeszcze nie zaczal ataku na gracza w danej fali wrogow
            }
            bool laser = false, rakiet = false;
            if (GlobalAcc.StatekGracza.Zniszczony == false)
            {
                if (RakietyOd != null && UzywaRakieta == true)
                    for (int i = 0; i < RakietyOd.Length; i++)
                        if (T > RakietyOd[i] && T < RakietyDo[i])
                        {
                            if (T % CzestRakiet == 0) rakiet = true;
                            break;
                        }

                if (LaseryOd != null && UzywaLaser == true)
                    for (int i = 0; i < LaseryOd.Length; i++)
                        if (T > LaseryOd[i] && T < LaseryDo[i])
                        {
                            if (T % CzestLaser == 0) laser = true;
                            break;
                        }
            }
            Statek.Pracuj(laser,rakiet);
            T = (int)Statek.AktT;
        }

        public void Rysuj(ref SpriteBatch SB)
        {
            if(T >= 1 && T <= KT)
            Statek.Rysuj(ref SB);
        }
    }

    class FalaWrogow
    {
        public StatekInstancja[] Instancje;
        public bool Koniec { protected set; get; }
        public FalaWrogow(FalaInformacyjna FI)
        {
            Instancje = new StatekInstancja[FI.Statki.Count];
            for (int i = 0; i < Instancje.Length; i++)
            {
                Instancje[i] = new StatekInstancja(FI.Statki[i]);
                Instancje[i].Statek.RuchSciezka(FI.DrogaX, FI.DrogaY, FI.T);
                GlobalAcc.ListaKolizji.Add(Instancje[i].Statek);
            }
        }


        public void Pracuj()
        {
            bool wszystkie_koniec = true;
            for (int i = 0; i < Instancje.Length; i++)
            {
                Instancje[i].Pracuj();
                if (Instancje[i].T <= Instancje[i].KT && Instancje[i].Statek.Zniszczony == false) wszystkie_koniec = false;
            }
            if (wszystkie_koniec) Koniec = true;
        }

        public void Rysuj(ref SpriteBatch SB)
        {
            for (int i = 0; i < Instancje.Length; i++)
                Instancje[i].Rysuj(ref SB);
        }
    }
}