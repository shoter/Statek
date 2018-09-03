using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Damian.MWGK.Obiekt;
namespace Damian.MWGK.Particle
{
    class DynamicParticle : Particle
    {
        private Vector2 Normalna;
        public Vector2 Dokad { private set; get; }
        public Vector2 Pozycja { private set; get; }
        public float V, A, Vmax;
        public Vector2 Przesuniecie { private set; get; }
        public Rodzaj_Ruchu Ruch { private set; get; }
        public Color KKolor { private set; get; } //koncowy kolor

        /// <summary>
        /// Konstruktor dynamicznej czasteczki ktora moze zmieniac swoja pozycje oraz kolor w trakcie czasu t
        /// </summary>
        /// <param name="x">pozycja x lewego gorneg rogu czateczki</param>
        /// <param name="y">pozycja y lewego gornego rogu czasteczki</param>
        /// <param name="szer">szerokosc czasteczki</param>
        /// <param name="wys">wysokosc czasteczki</param>
        /// <param name="kolor">kolor poczatkowy czasteczki</param>
        /// <param name="kkolor">kolor koncowy czasteczki</param>
        /// <param name="t">czas w ktorym czasteczka bedzie sie zmieniac</param>
        public DynamicParticle(int x,int y,int szer,int wys,Color kolor,
                               Color kkolor,int t) :base(x,y,szer,wys,kolor,t)
        {
            Dokad = new Vector2(0f, 0f);
            Pozycja = new Vector2(x, y);
            KKolor = kkolor;
            V = 0f;
            A = 0f;
            Vmax = 1f;
        }

        /// <summary>
        /// Dokad ma sie udac czasteczka ruchem jednostajnym(czasteczka sama obliczy predkosc ruchu)
        /// </summary>
        /// <param name="dokad">pozycja do ktorej uda sie czasteczka</param>
        public void RuchJednostajny(Vector2 dokad)
        {
            Vector2 pozycja = new Vector2(Polozenie.X,Polozenie.Y);
            Vector2 roznica = dokad - pozycja;

            float odleglosc = roznica.Length();
            int t = KT - T; //pozostaly czas
            V = odleglosc / t; //V = s / t

            //normalizujemy i dajemy go do normalnej
            roznica.Normalize();
            Normalna = roznica;

            //upewniamy sie ze to ruch jednotajny
            A = 0f;
            Ruch = Rodzaj_Ruchu.RuchJednostajny;
        }
        /// <summary>
        /// Dokad ma sie udac czasteczka ruchem jednostajnym
        /// </summary>
        /// <param name="dokad">pozycja do ktorej uda sie czasteczka</param>
        /// <param name="v">predkosc czasteczki</param>
        public void RuchJednostajny(Vector2 dokad, float v)
        {
            Vector2 Pozycja = new Vector2(Polozenie.X, Polozenie.Y);
            Vector2 roznica = dokad - Pozycja;
            V = v;
            //normalizujemy i dajemy go do normalnej
            roznica.Normalize();
            Normalna = roznica;

            //upewniamy sie ze to ruch jednotajny
            A = 0f;
            Ruch = Rodzaj_Ruchu.RuchJednostajny;
        }
        /// <summary>
        /// Dokad ma sie udac czasteczka ruchem przyspieszonym(czasteczka sama obliczy predkosc ruchu)
        /// jesli cialo mialo jakies V0 to zostanie wliczona poprawka A do tej predkosci
        /// cialo doleci w czasie kt - t do celu
        /// </summary>
        /// <param name="dokad">pozycja do ktorej uda sie czasteczka</param>
        public void RuchPrzyspieszony(Vector2 dokad)
        {
            //jesli a bedzie mniejsze od 0 to nie zostanie nadane przyspieszenie i ruch bedzie jednostajny
            Vector2 roznica = dokad - Pozycja;

            float odleglosc = roznica.Length();
            int t = KT - T; //pozostaly czas
            A = 2 * (odleglosc - V * t) / (t * t);
            if (A < 0f) A = 0f;

            //normalizujemy i dajemy go do normalnej
            roznica.Normalize();
            Normalna = roznica;

            if (A != 0f)
                Ruch = Rodzaj_Ruchu.RuchPrzyspieszony;
            else
                Ruch = Rodzaj_Ruchu.RuchJednostajny;
        }
        /// <summary>
        /// Dokad ma sie udac czasteczka ruchem przyspieszonym(czasteczka sama obliczy predkosc ruchu)
        /// jesli cialo mialo jakies V0 to zostanie nadpisane
        /// cialo doleci w czasie kt - t do celu
        /// </summary>
        /// <param name="dokad">pozycja do ktorej uda sie czasteczka</param>
        /// <param name="V">predkosc V0 ciala w tym ruchu</param>
        public void RuchPrzyspieszony(Vector2 dokad, float v)
        {
            //jesli a bedzie mniejsze od 0 to nie zostanie nadane przyspieszenie i ruch bedzie jednostajny
            Vector2 roznica = dokad - Pozycja;
            V = v;

            float odleglosc = roznica.Length();
            int t = KT - T; //pozostaly czas
            A = 2 * (odleglosc - V * t) / (t * t);
            if (A < 0f) A = 0f;

            //normalizujemy i dajemy go do normalnej
            roznica.Normalize();
            Normalna = roznica;

            if (A != 0f)
                Ruch = Rodzaj_Ruchu.RuchPrzyspieszony;
            else
                Ruch = Rodzaj_Ruchu.RuchJednostajny;
        }
        /// <summary>
        /// Ruch przyspieszony w ktorym sami nadajemy v i a niezalezne od t
        /// </summary>
        /// <param name="dokad">pozycja w strone ktorej cialo bedzie sie poruszac</param>
        /// <param name="v">predkosc poczatkowa</param>
        /// <param name="a">przyspieszenie</param>
        public void RuchPrzyspieszony(Vector2 dokad, float v, float a)
        {
            if (a != 0f)
                Ruch = Rodzaj_Ruchu.RuchPrzyspieszony;
            else
                Ruch = Rodzaj_Ruchu.RuchJednostajny;
            Vector2 pozycja = new Vector2(Polozenie.X, Polozenie.Y);
            Vector2 roznica = dokad - pozycja;

            V = v;
            A = a;

            //normalizujemy i dajemy go do normalnej
            roznica.Normalize();
            Normalna = roznica;
        }
        /// <summary>
        /// Nadaje ruch opozniony w ktorym cialo ma dotrzec do dokad w czasie kt - t
        /// </summary>
        /// <param name="dokad">pozycja do ktorej ma sie udac cialo</param>
        public void RuchOpozniony(Vector2 dokad)
        {
            //jesli a bedzie wieksze od 0 to nie zostanie nadane przyspieszenie i ruch bedzie jednostajny
            Vector2 roznica = dokad - Pozycja;

            float odleglosc = roznica.Length();
            int t = KT - T; //pozostaly czas
            A = -2 * (odleglosc - V * t) / (t * t);

            if (A > 0f) A = 0f;

            //normalizujemy i dajemy go do normalnej
            roznica.Normalize();
            Normalna = roznica;

            if (A != 0f)
                Ruch = Rodzaj_Ruchu.RuchOpozniony;
            else
                Ruch = Rodzaj_Ruchu.RuchJednostajny;
        }

        public override void Pracuj()
        {
            if (!Przyczepiony)
            {
                Przesuniecie = Normalna * V;
                Pozycja += Przesuniecie;
                Polozenie.X = (int)Pozycja.X;
                Polozenie.Y = (int)Pozycja.Y;
                V += A;
            }
            else
                PrzyczepieniePraca();

            if (V > Vmax) V = Vmax;

            T++;
            if (T == KT) Koniec = true;
        }

        public override void Rysuj(ref SpriteBatch SB)
        {
            Color asdas = Color.Lerp(Kolor,KKolor,(float)T/KT);
            float alpha = (float)asdas.A / 255;
            asdas.R = (byte)(asdas.R *alpha);
            asdas.G = (byte)(asdas.G * alpha);
            asdas.B = (byte)(asdas.B * alpha);
            SB.Draw(MalaTekstura,Polozenie,asdas);
        }

    }

}