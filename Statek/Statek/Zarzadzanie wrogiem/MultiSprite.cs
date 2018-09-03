using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Damian.MWGK.Sprite
{
    class MultiSprite
    {
        protected Texture2D Obrazek;
        /// <summary>
        /// Szer pojedynczego segmentu
        /// </summary>
        public int Szer { protected set; get; }
        /// <summary>
        /// Wys pojedynczego segmentu
        /// </summary>
        public int Wys { protected set; get; }
        /// <summary>
        /// Zwraca nam ilosc sprite'ów w poziomie
        /// </summary>
        public int IloscX { protected set; get; }
        /// <summary>
        /// zwraca nam iloœæ sprite'ów w pionie
        /// </summary>
        public int IloscY { protected set; get; }
        public int PrzesuniecieX { protected set; get; }
        public int PrzesuniecieY { protected set; get; }

        public MultiSprite(ref Texture2D obrazek, int szer, int wys, int iloscx, int iloscy, int przesx, int przesy)
        {
            Obrazek = obrazek;
            Szer = szer;
            Wys = wys;
            IloscX = iloscx;
            IloscY = iloscy;
            PrzesuniecieX = przesx;
            PrzesuniecieY = przesy;
        }

        public MultiSprite(MultiSprite MS)
        {
            Obrazek = MS.Obrazek;
            Szer = MS.Szer;
            Wys = MS.Wys;
            IloscX = MS.IloscX;
            IloscY = MS.IloscY;
            PrzesuniecieX = MS.PrzesuniecieX;
            PrzesuniecieY = MS.PrzesuniecieY;
        }

        public void Rysuj(int x, int y, int elementx, int elementy, ref SpriteBatch SB)
        {
            SB.Draw(Obrazek, new Rectangle(x, y, Szer, Wys), new Rectangle((Szer + PrzesuniecieX) * elementx + PrzesuniecieX, (Wys + PrzesuniecieY) * elementy + PrzesuniecieY, Szer, Wys), Color.White);
        }

        public virtual void Pracuj() { }
       

    }
}
