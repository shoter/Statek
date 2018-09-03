using System;
using Damian.MWGK.Global.Test;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Damian.MWGK.Dodatki.Gwiazdy
{
    class Gwiazda
    {
        Texture2D Tekstura;
        Vector2 Polozenie;
        float Nasycenie; //kolorem bia³ym,gwiazdki bêd¹ ³adnie b³yszczeæ
        bool Rosnie;

        public Gwiazda(Vector2 Gdzie,int rozmiar,GraphicsDevice GD)
        {
            Polozenie = Gdzie;
            Tekstura = new Texture2D(GD,rozmiar,rozmiar);
            Rosnie = false;
            Nasycenie = 1f;
            Color[] pixele = new Color[rozmiar * rozmiar];
            for(int y = 0;y < rozmiar;y++)
                for(int x = 0;x < rozmiar;x++)
                {

                    double odl = Math.Sqrt( Math.Pow(x - rozmiar/2,2) + Math.Pow(y - rozmiar/2,2));
                    byte nasycenie = 0;
                    if(odl <= rozmiar/2)
                    nasycenie = (byte)( Math.Abs(1f - odl / rozmiar*2) * 255f);
                    pixele[y * rozmiar + x] = new Color(nasycenie, nasycenie, nasycenie, nasycenie);
                }
            Tekstura.SetData<Color>(pixele);
        }

        public void Rysuj(ref SpriteBatch SB)
        {
            Color maska;
            byte kol = (byte)(Nasycenie * 255f);
            maska = new Color(kol,kol,kol / 3 * 2,kol);
        SB.Draw(Tekstura,Polozenie,maska);
        }

        public void Pracuj()
        {
            
            Random rand = new Random();
            float zmiana = (float)rand.Next(1, 20) / 1000f;
            if (Rosnie == false) zmiana = -zmiana;
            Nasycenie += zmiana;
            if (Nasycenie < 0.6)
                Rosnie = true;
            if (Nasycenie > 1f)
            {
                Rosnie = false;
                Nasycenie = 1f;
            }

            Polozenie.Y += rand.Next(1, 20) / 100f;
            if (Polozenie.Y > 800f)
            {
                Polozenie.Y = -10f;
                Polozenie.X = rand.Next(0, 480);
            }

        }
    }
}
