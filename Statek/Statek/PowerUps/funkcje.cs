using Microsoft.Xna.Framework;

namespace Damian.MWGK.funkcje
{
    class Funkcje
    {

        public static  bool kolizja(Vector2 p1, int s1, int w1, Vector2 p2, int s2, int w2)
        {

            float x,y;
            x = p1.X + s1;
            y = p1.Y;
            if ((x >= p2.X && x <= p2.X + s2 &&
                y >= p2.Y && y <= p2.Y + w2)) return true;
            //-----------------------
            x = p1.X;
            if ((x >= p2.X && x <= p2.X + s2 &&
                y >= p2.Y && y <= p2.Y + w2)) return true;
            //---------------
            y += w1;
            if ((x >= p2.X && x <= p2.X + s2 &&
                y >= p2.Y && y <= p2.Y + w2)) return true;

            x = p1.X + s1;
            if ((x >= p2.X && x <= p2.X + s2 &&
                y >= p2.Y && y <= p2.Y + w2)) return true;
            return false;
        }
    }
}