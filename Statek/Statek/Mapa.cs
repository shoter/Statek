using Damian.MWGK.Fala;
using Damian.MWGK.Global.Zbiory;
using Damian.MWGK.Global.Test;
using System.Collections.Generic;
using Damian.MWGK.Obiekt.Statek;
namespace Damian.MWGK.Map
{
    class Mapa
    {
        protected int T, KT; //trwanie danej mapy
        protected bool Boss; //jesli ostatnia fala jest bossem to true i KT sie nie liczy
        public List<FalaInformacyjna> FI;
        public List<int> KiedyFala; //musza byc dodawane w tej samej kolejnosci
        protected int i = 0; //na ktorej fali jest skrypt 
        FalaWrogow LastAdd; //ostatnio dodana

        public Mapa(int t, bool boss, List<FalaInformacyjna> fi,List<int> kiedyfala)
        {
            T = 0;
            KT = t;
            Boss = boss;
            FI = fi;
            KiedyFala = kiedyfala;
        }
        public Mapa(int t, bool boss)
        {
            T = 0;
            KT = t;
            Boss = boss;
            FI = new List<FalaInformacyjna>();
            KiedyFala = new List<int>();
        }

        public void Pracuj()
        {
            if (i >= FI.Count) return;
            if (T >= KiedyFala[i])
            {
                LastAdd = new FalaWrogow(FI[i]);
                GlobalAcc.ListaWrogow.Add(LastAdd);
                i++;
            }
            T++;
        }

        public bool WalczyZBossem()
        {
            if (Boss && i >= FI.Count) return true;
            return false;
        }
        public StatekWroga ZwrocBossa()
        {
            if (!WalczyZBossem()) return null;
            else return LastAdd.Instancje[0].Statek;
        }

        public bool KoniecMapy()
        {
            if ( i >= FI.Count && LastAdd != null &&  LastAdd.Koniec ) return true;
            return false;
        }

    }

}