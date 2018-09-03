using Damian.MWGK.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Damian.MWGK.Sprite
{
    class SpriteAnimated : MultiSprite
    {
        public int X { protected set; get; }
        public int Y { protected set; get; }
        protected int T,KT;
        int Ilosc;
        public bool Koniec; 
        public SpriteAnimated(int x,int y,ref Texture2D obrazek, int szer, int wys, int iloscx, int iloscy, int przesx, int przesy, int t)
           :base(ref obrazek,szer,wys,iloscx,iloscy,przesx,przesy)
       {
           T = 0;
           KT = t;
           Ilosc = IloscX * IloscY;
           X = x; Y = y;
            
       }


    public SpriteAnimated(int x, int y, MultiSprite MS, int t)
            :base(MS)
    {
        T = 0;
        KT = t;
        Ilosc = IloscX * IloscY;
        X = x - Szer / 2;Y = y- Wys /2;
    }


       public override void Pracuj()
       {
            /*
           Jak to dziala , przyklad
             * 01|02|03|04|05
             * 06|07|08|09|10
             * 11|12|13|14|15
             * koniec przykładu :D
             * 
           */
           T++;
           if (T >= KT) Koniec = true;
           

       }
       public void Rysuj(ref SpriteBatch SB)
       {
           int ktory = (int)((float)T / KT * Ilosc);
           int koly /*kolumna Y*/ = ktory / IloscX;
           int kolx = ktory % IloscX;
           base.Rysuj(X, Y, kolx, koly, ref SB);
       } 
    }

}