using Damian.MWGK.Particle;
using Damian.MWGK.Particle.Storage;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Damian.MWGK.Particle.System
{
    struct ParticleSystemComponent
    {
        public bool Pracuj;
        public bool Rysuj;
        public ParticleStorage PS;
        public ParticleSystemComponent(bool pracuj, bool rysuj, ParticleStorage ps)
        {
            Pracuj = pracuj;
            Rysuj = rysuj;
            PS = ps;
        }
    }

    class ParticleSystem
    {
        public static void Inicjalizuj()
        {
            Kontenery = new List<ParticleSystemComponent>();
        }
       
        private static List<ParticleSystemComponent> Kontenery;
        
        public static void Pracuj()
        {
            foreach (ParticleSystemComponent X in Kontenery)
                if(X.Pracuj)
                X.PS.Pracuj(true);

            for (int i = 0; i < Kontenery.Count; i++)
                if (Kontenery[i].Pracuj && Kontenery[i].PS.Koniec && Kontenery[i].PS.pusty())
                    Kontenery.RemoveAt(i);
        }

        public static void Rysuj(ref SpriteBatch SB)
        {
            foreach (ParticleSystemComponent X in Kontenery)
                if(X.Rysuj)
                X.PS.Rysuj(ref SB);
        }

        public static void dodaj(ParticleSystemComponent PSC)
        {
        Kontenery.Add(PSC);
        }

        public static void Czysc()
        {
            for (int i = 0; i < Kontenery.Count; i++)
                Kontenery[i].PS.Czysc();

        }


    }
}