using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Damian.MWGK.Obiekt;
using Damian.MWGK.Obiekt.Bron;

namespace Damian.MWGK.Global.Zbiory //zbiory wyposazen,grafik etc
{
    class DzialoRakietowe
    {
        public Rakieta Pierwowzor;
        public int Czestotliwosc; //strzalu
        public int T;
        public bool Gotowy;
        public void Pracuj()
        {
            T++;
            if (T > Czestotliwosc)
            {
                T = 0;
                Gotowy = true;
            }
        }

        public DzialoRakietowe(Rakieta p, int czest)
        {
            Pierwowzor = p;
            Czestotliwosc = czest;
            T = 0;
            Gotowy = false;
        }

        public DzialoRakietowe Kopia()
        {
            DzialoRakietowe ret = new DzialoRakietowe(Pierwowzor, Czestotliwosc);
            return ret;
        }

        public bool Strzel()
        {
            if (Gotowy)
            {
                Gotowy = false;
                return true;
            }
            return false;
        }

    }
    class DzialoLaserowe
    {
        public Laser Pierwowzor;
        public int Czestotliwosc; //strzalu
        public int T;
        public bool Gotowy;
        public void Pracuj()
        {
            T++;
            if (T > Czestotliwosc)
            {
                T = 0;
                Gotowy = true;
            }
        }

        public DzialoLaserowe(Laser p, int czest)
        {
            Pierwowzor = p;
            Czestotliwosc = czest;
            T = 0;
            Gotowy = false;
        }

        public DzialoLaserowe Kopia()
        {
            DzialoLaserowe ret = new DzialoLaserowe(Pierwowzor, Czestotliwosc);
            return ret;
        }
        public bool Strzel()
        {
            if (Gotowy)
            {
                Gotowy = false;
                return true;
            }
            return false;
        }

    }

      

    class Wyposazenie
    {

        public static DzialoRakietowe[] Rakiety;
        public static DzialoLaserowe[] Lasery;

        public static void Inicjalizacja(ContentManager CM)
        {
        Rakiety = new DzialoRakietowe[3];
            Texture2D RakietaID1 = CM.Load<Texture2D>("RakietaID1");
            Rakiety[0] = new DzialoRakietowe(new Rakieta(Vector2.Zero, ref RakietaID1, 0f, 30f,
                new Particle.Storage.ParticleGun(0, 0, 2, 4, 2, 4, Color.OrangeRed, Color.Yellow, Color.Red, Color.OrangeRed, -MathHelper.PiOver4, MathHelper.PiOver4, 20f, 75f, 5, 15, 0, Rodzaj_Ruchu.RuchJednostajny, 4, Particle.Storage.Rodzaj_Dziala.Losowo),
                new Particle.Storage.ParticleGun(0, 0, 2, 4, 2, 4, Color.Gray, Color.Gray, Color.Transparent, Color.Transparent, -MathHelper.PiOver4, MathHelper.PiOver4, 50f, 150f, 5, 25, 0, Rodzaj_Ruchu.RuchJednostajny, 1, Particle.Storage.Rodzaj_Dziala.Losowo),
                7f, 5, 0.03f), 20);

              Rakiety[1] = new DzialoRakietowe(new Rakieta(Vector2.Zero, ref RakietaID1, 0f, 15f, // RAKIETA ID1 WROGA!!!
                new Particle.Storage.ParticleGun(0, 0, 2, 4, 2, 4, Color.Yellow, Color.Orange, Color.Red, Color.OrangeRed, -MathHelper.PiOver4, MathHelper.PiOver4, 20f, 75f, 5, 10, 0, Rodzaj_Ruchu.RuchJednostajny, 1, Particle.Storage.Rodzaj_Dziala.Losowo),
                new Particle.Storage.ParticleGun(0, 0, 2, 4, 2, 4, Color.Gray, Color.Gray, Color.Transparent, Color.Transparent, -MathHelper.PiOver4, MathHelper.PiOver4, 50f, 150f, 5, 20, 0, Rodzaj_Ruchu.RuchJednostajny, 1, Particle.Storage.Rodzaj_Dziala.Losowo),
                5f, 5, 0.03f), 20);



            Lasery = new DzialoLaserowe[4];
            Texture2D LaserID1 = CM.Load<Texture2D>("Lasery\\LaserLvl1");
            Texture2D LaserID2 = CM.Load<Texture2D>("Lasery\\LaserLvl2");
            Texture2D LaserID3 = CM.Load<Texture2D>("Lasery\\LaserLvl3");
            Texture2D LaserID4 = CM.Load<Texture2D>("Lasery\\LaserZielony");
            Lasery[0] = new DzialoLaserowe(new Laser(Vector2.Zero, ref LaserID1, 0f
                , 10f, Vector2.Zero, 12f, new Color(0,143,255)), 15);
            Lasery[1] = new DzialoLaserowe(new Laser(Vector2.Zero, ref LaserID2, 0f
                , 20f, Vector2.Zero, 11f, new Color(0, 143, 255)), 14);
            Lasery[2] = new DzialoLaserowe(new Laser(Vector2.Zero, ref LaserID3, 0f
                , 40f, Vector2.Zero, 13f, new Color(0, 143, 255)), 17);

            Lasery[3] = new DzialoLaserowe(new Laser(Vector2.Zero, ref LaserID4, 0f, 2f, Vector2.Zero, 12f,Color.Green), 10);
        }

    }
}

 