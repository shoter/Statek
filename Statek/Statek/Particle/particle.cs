using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Damian.MWGK.Global.Test;
using Damian.MWGK.Obiekt;

namespace Damian.MWGK.Particle
{
    /// <summary>
    /// Jest to czasteczka
    /// </summary>
    class Particle
    {
        protected Rectangle Polozenie;
        public Color Kolor;
        protected static RenderTarget2D MalaTekstura;
        public int T { protected set; get; }
        public int KT { protected set; get; }
        public bool Koniec { protected set; get; }

        /// <summary>
        /// Powoduje przyczepienie sie do jakiegos obiektu
        /// </summary>
        protected bool Przyczepiony = false; //dziala tylko wtedy kiedy czasteczka nie ma ruchu
        protected Obiekt.Obiekt PrzyczepionyObiekt; //do jakiego obiektu przyczpiony
        protected Vector2 PolozeniePrzyczepienia; //jak jest polozony wzgledem obiektu

        /// <summary>
        /// Konstruktor czasteczki klasy podstawowej
        /// </summary>
        /// <param name="x">pozycja x lewego gornego rogu czasteczki</param>
        /// <param name="y">pozycja y lewego gornego rogu czasteczki</param>
        /// <param name="szer">szerokosc</param>
        /// <param name="wys">wysokosc</param>
        /// <param name="kolor">kolor</param>
        public Particle(int x, int y, int szer, int wys, Color kolor,int t)
        {
            Polozenie.X = x;
            Polozenie.Y = y;
            Polozenie.Width = szer;
            Polozenie.Height = wys;

            T = 0;
            KT = t;
            if (KT < 1) KT = 1;

            Kolor = kolor;

            Koniec = false;
            GlobalAcc.Czasteczki++;
        }

        public void Przyczep(Obiekt.Obiekt Co)
        {
            Przyczepiony = true;
            PrzyczepionyObiekt = Co;
            PolozeniePrzyczepienia = Co.Pozycja - new Vector2(Polozenie.X, Polozenie.Y);
        }

        protected void PrzyczepieniePraca()
        {
            Vector2 NowaPozycja = PrzyczepionyObiekt.Pozycja - PolozeniePrzyczepienia;
            Polozenie.X = (int)NowaPozycja.X;
            Polozenie.Y = (int)NowaPozycja.Y;
        }

        public static void inicjalizuj(GraphicsDevice GD)
        {
            MalaTekstura = new RenderTarget2D(GD, 1, 1);
            GD.SetRenderTarget(MalaTekstura);
            GD.Clear(Color.White);
            GD.SetRenderTarget(null);
        }
        public virtual void Rysuj(ref SpriteBatch SB)
        {
            SB.Draw(MalaTekstura, Polozenie, Kolor);
        }
        public virtual void Pracuj()
        {
            T++;
            if (T == KT) Koniec = true;
        }
    }
}