using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Damian.MWGK.Particle;
namespace Damian.MWGK.Particle.Storage
{
    class ParticleStorage
    {
       protected List<Particle> Obiekty;
       public bool Koniec { protected set; get; }

       public ParticleStorage()
       {
           Obiekty = new List<Particle>();
           Koniec = false;
       }

        /// <summary>
        /// dodaje do listy obiektow czasteczke ktora zostanie umieszczona na koncu listy wyswietlania
        /// </summary>
        /// <param name="czasteczka">czasteczka ktora ma byc dodana</param>
       public virtual void Dodaj(Particle czasteczka)
       {
           Obiekty.Add(czasteczka);           
       }

       public bool pusty()
       {
           if (Obiekty.Count == 0) return true;
           return false;
       }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TworzCzasteczki">Najlpiej daj true</param>
       public virtual void Pracuj(bool TworzCzasteczki)
       {
           if (Obiekty.Count == 0)
           {
               Koniec = true;
               return;
           }
            foreach(Particle X in Obiekty)
            {
                X.Pracuj();
                
            }
            for (int i = 0; i < Obiekty.Count; i++)
            {
                if (Obiekty[i].Koniec) Obiekty.RemoveAt(i);
            }
         
       }

       public virtual void Rysuj(ref SpriteBatch SB)
       {
           foreach (Particle X in Obiekty)
           {
               X.Rysuj(ref SB);
           }
       }

       public virtual void Czysc()
       {
           Obiekty.RemoveRange(0, Obiekty.Count);
       }
       

    }
}