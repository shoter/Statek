using Damian.MWGK.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Damian.MWGK.Dodatki.Gwiazdy;
using System;
using Damian.MWGK.Particle.System;
using Damian.MWGK.Global;
using Damian.MWGK.Global.Test;
using Damian.MWGK.Global.Graphics;
using Damian.MWGK.Global.Zbiory;
using Damian.MWGK.Fala;
using Damian.MWGK.Obiekt.Statek;
using System.Collections.Generic;
using Damian.MWGK.Obiekt.Powers;
using Damian.MWGK.Obiekt;
using Damian.MWGK.Particle.Storage;

namespace Damian.MWGK.Global.Zbiory
{
    enum RodzajeTorow : byte
    {
       LewaGornaXSrodekXGornaPrawaYNormalT = 0,
       PrawaGornaXSordekXLewaGornaYNormalT = 1,
       GornySrodekXDolnySrodekYSlowT = 2,
       LewyGornyXGlebokiXPrawyGornyYNormalT = 3,
       PrawyGornyXGlebokiXLewyGornyYNormalT = 4,
       PoziomYNormalT = 5,
       SciezkaBossa = 6
       
    }

    enum RodzajeFal : byte
    {
        RakietowaNamierzanaSlaba = 0,
        LaserowaNamierzalnaSlaba = 1,
        LaserNamierzalnaRakietowaNamierzalnaSlaba = 2,
        LaserNamierzalnaRakietowaNamierzalnaSlabaSzybka = 4,
        LaserNamierzalnyRakietyNienamierzalneSredni = 3,
        BOSS = 5
    }

    enum RodzajeStatkow : byte
    {
        //LH - slaby kadlub
        //MH - sredni kadlub
        //HH - silny kadlub
        //LS - slabe tarcze
        //MS - srednie tarcze
        //HS - silne tarcze
        LHLS = 0,
        MHMS = 1,
    }


    class Mapy //glowna funkcja odpowiadajaca za gre hell yeah,nareszcie sie tu dostalem
    {
        static protected Mapa[] ListaMap = new Mapa[3];
        static int aktualna = 0;
        static int aktualnaWyswietlana = 0;
        static Gwiazda[] gwiazdy;
        static int ile_gwiazd = 40;


        static Vector2 PozycjaPlanety; //losowana z przedzialu ekranu


        static int IloscTorow = 7;
        static Curve[] ToryX = new Curve[IloscTorow];
        static Curve[] ToryY = new Curve[IloscTorow];
        static int[] ToryT = new int[IloscTorow];
        static int[][] ToryLaseryOd = new int[IloscTorow][];
        static int[][] ToryLaseryDo = new int[IloscTorow][];
        static int[][] ToryRakietyOd = new int[IloscTorow][];
        static int[][] ToryRakietyDo = new int[IloscTorow][];

        static int IloscFal = 10;
        static FalaInformacyjna[] Fale = new FalaInformacyjna[IloscFal];

        static int IloscStatkow = 6;
        static StatekWroga[] Statki = new StatekWroga[IloscStatkow];

        static int Przerwa = 50; //przerwa miedzy levelami


        static GraphicsDevice GD;
        static int SzerEkranu, WysEkranu;
        static Random rand = new Random();

        public static void Init(GraphicsDevice gd)
        {
            GD = gd;
            Viewport viewport = gd.Viewport;
            SzerEkranu = viewport.Width;
            WysEkranu = viewport.Height;
            GenerujGwiazdy();
            InitTory();
            InitStatki();
            InitFal();
            MapInit();
          

            PozycjaPlanety = new Vector2(rand.Next(0, 480), rand.Next(0, 800));
            

           
        }

        public static void Rysuj(ref SpriteBatch SB)
        {
            
            
            
            for (int i = 0; i < ile_gwiazd; i++)
                gwiazdy[i].Rysuj(ref SB);
            SB.Draw(Grafika.Planety[aktualna], Grafika.PolozeniePlanet[aktualna], Color.White);
            GlobalAcc.Rysuj(ref SB);
            ParticleSystem.Rysuj(ref SB);
            GlobalAcc.StatekGracza.Rysuj(ref SB);
            if (ListaMap[aktualna].WalczyZBossem())
            {
                StatekWroga boss = ListaMap[aktualna].ZwrocBossa();
                int ile_tarczy = (int)((float)boss.AktTarcza / boss.MaxTarcza * 250);
                int ile_kadluba = (int)((float)boss.AktKadlub / boss.MaxKadlub * 250);
                SB.Draw(Grafika.Empty, new Rectangle(240 - ile_kadluba / 2, 0, ile_kadluba, 15), Color.Red);
                SB.Draw(Grafika.Empty, new Rectangle(240 - ile_tarczy / 2, 15, ile_tarczy, 15), Color.Blue);
            }
            if (Przerwa != 100)
            {
                string NazwaLvl = "Level " + (aktualnaWyswietlana + 1);
                Color przyciemnienie;
                if (Przerwa < 50)
                {
                    int kol = (int)(Przerwa / 50.0 * 255.0);
                    przyciemnienie = new Color(0, 0, 0, kol);
                }
                else
                {
                    int kol = (int)(255.0 - (Przerwa - 50.0) / 50.0 * 255.0);
                    przyciemnienie = new Color(0, 0, 0, kol);
                }
                SB.Draw(Grafika.Empty, new Rectangle(0,0,480,800), przyciemnienie);
                Vector2 RozmiarTekstu = Grafika.FontNazwaLvl.MeasureString(NazwaLvl);
                przyciemnienie = new Color(przyciemnienie.A, przyciemnienie.A, przyciemnienie.A, przyciemnienie.A);
                SB.DrawString(Grafika.FontNazwaLvl,NazwaLvl, new Vector2(240f, 400f) - RozmiarTekstu / 2,przyciemnienie);

            }
            
        }

        //dla tej funkcji zmienne dodatkowe
        static bool RozkazWydany = false; //rozkaz ruchu
        public static void Pracuj(Vector2[] klikniecia,int ile)
        {
            GlobalAcc.Pracuj();
            if (Przerwa == 100)
            {
                if (ListaMap[aktualna].KoniecMapy() == false)
                {
                    foreach (Gwiazda X in gwiazdy)
                        X.Pracuj();
                    ListaMap[aktualna].Pracuj();
                    GlobalAcc.StatekGracza.Pracuj(klikniecia, ile, true, true);
                }
                else
                {
                    if (RozkazWydany == false)
                    {
                        GlobalAcc.StatekGracza.RuchPrzyspieszony(new Vector2(GlobalAcc.StatekGracza.X, -400f), 7f, 1f, true);
                        GlobalAcc.StatekGracza.Normalna = new Vector2(0, -1);
                        RozkazWydany = true;
                    }
                    else
                    {
                        GlobalAcc.StatekGracza.Pracuj();
                        if (GlobalAcc.StatekGracza.Y < -200f)
                        {
                            ParticleSystem.Czysc();
                            GlobalAcc.StatekGracza.Czysc();
                            Przerwa = 0;
                            aktualnaWyswietlana++;
                            

                        }
                    }

                }
            }
            else
            {
                Przerwa++;
            }
            if (Przerwa == 50)
            {
                GlobalAcc.Clear();
                GlobalAcc.StatekGracza.Pozycja = new Vector2(230f, 600f);
                GlobalAcc.StatekGracza.V = 7f;
                GlobalAcc.StatekGracza.A = 0f;
                if(aktualna != ListaMap.Length - 1)
                aktualna++;
                RozkazWydany = false;
                
            }
            
        }

        protected static void GenerujGwiazdy()
        {
            gwiazdy = new Gwiazda[ile_gwiazd];
            for (int i = 0; i < ile_gwiazd; i++)
                gwiazdy[i] = new Gwiazda(new Vector2(rand.Next(0, SzerEkranu), rand.Next(0, WysEkranu)), rand.Next(1, 8), GD);
        }

        protected static void InitTory()
        {
            //-----------------------LewaGornaXSrodekXGornaPrawaYNormalT-------------------
            Curve DrogaX, DrogaY;
            DrogaX = new Curve();
            DrogaY = new Curve();
            DrogaX.Keys.Add(new CurveKey(0, -30f));
            DrogaX.Keys.Add(new CurveKey(150, 300f));
            DrogaX.Keys.Add(new CurveKey(300, 600f));
            DrogaY.Keys.Add(new CurveKey(0,-30f));
            DrogaY.Keys.Add(new CurveKey(150, 300f));
            DrogaY.Keys.Add(new CurveKey(300, -100f));
            DrogaX.ComputeTangents(CurveTangent.Smooth);
            DrogaY.ComputeTangents(CurveTangent.Smooth);
            ToryX[0] = DrogaX;
            ToryY[0] = DrogaY;
            ToryT[0] = 300;
            ToryLaseryOd[0] = new int[1]; ToryLaseryOd[0][0] = 100;
            ToryLaseryDo[0] = new int[1]; ToryLaseryDo[0][0] = 200;
            ToryRakietyOd[0] = new int[1]; ToryRakietyOd[0][0] = 100;
            ToryRakietyDo[0] = new int[1]; ToryRakietyDo[0][0] = 200;
            //-----------------------PrawaGornaXSordekXLewaGornaYNormalT-------------------
            DrogaX = new Curve();
            DrogaY = new Curve();
            DrogaX.Keys.Add(new CurveKey(0, 510f));
            DrogaX.Keys.Add(new CurveKey(150, 300f));
            DrogaX.Keys.Add(new CurveKey(300, 0f));
            DrogaY.Keys.Add(new CurveKey(0, -30f));
            DrogaY.Keys.Add(new CurveKey(150, 300f));
            DrogaY.Keys.Add(new CurveKey(300, -100f)); 
            DrogaX.ComputeTangents(CurveTangent.Smooth);
            DrogaY.ComputeTangents(CurveTangent.Smooth);
            ToryX[1] = DrogaX;
            ToryY[1] = DrogaY;
            ToryT[1] = 300;
            ToryLaseryOd[1] = new int[1]; ToryLaseryOd[1][0] = 100;
            ToryLaseryDo[1] = new int[1]; ToryLaseryDo[1][0] = 200;
            ToryRakietyOd[1] = new int[1]; ToryRakietyOd[1][0] = 100;
            ToryRakietyDo[1] = new int[1]; ToryRakietyDo[1][0] = 200;
            //-----------------------GornySrodekXDolnySrodekYSlowT-------------------
            DrogaX = new Curve();
            DrogaY = new Curve();
            DrogaX.Keys.Add(new CurveKey(0, 240f));
            DrogaX.Keys.Add(new CurveKey(400, 240f));
            DrogaY.Keys.Add(new CurveKey(0, -100f));
            DrogaY.Keys.Add(new CurveKey(400, 910f));
            DrogaX.ComputeTangents(CurveTangent.Smooth);
            DrogaY.ComputeTangents(CurveTangent.Smooth);
            ToryX[2] = DrogaX;
            ToryY[2] = DrogaY;
            ToryT[2] = 400;
            ToryLaseryOd[2] = new int[1]; ToryLaseryOd[2][0] = 100;
            ToryLaseryDo[2] = new int[1]; ToryLaseryDo[2][0] = 300;
            ToryRakietyOd[2] = new int[1]; ToryRakietyOd[2][0] = 100;
            ToryRakietyDo[2] = new int[1]; ToryRakietyDo[2][0] = 300;
            //-----------------------LewyGornyXGlebokiXPrawyGornyYNormalT-------------------
            DrogaX = new Curve();
            DrogaY = new Curve();
            DrogaX.Keys.Add(new CurveKey(0, 100f));
            DrogaX.Keys.Add(new CurveKey(150, 100f));
            DrogaX.Keys.Add(new CurveKey(250, 300f));
            DrogaX.Keys.Add(new CurveKey(400, 300f));

            DrogaY.Keys.Add(new CurveKey(0, -100f));
            DrogaY.Keys.Add(new CurveKey(150, 300f));
            DrogaY.Keys.Add(new CurveKey(250, 300f));
            DrogaY.Keys.Add(new CurveKey(400,-100f));
            DrogaX.ComputeTangents(CurveTangent.Smooth);
            DrogaY.ComputeTangents(CurveTangent.Smooth);
            ToryX[3] = DrogaX;
            ToryY[3] = DrogaY;
            ToryT[3] = 400;
            ToryLaseryOd[3] = new int[1]; ToryLaseryOd[3][0] = 150;
            ToryLaseryDo[3] = new int[1]; ToryLaseryDo[3][0] = 250;
            ToryRakietyOd[3] = new int[2]; ToryRakietyOd[3][0] = 0; ToryRakietyOd[3][1] = 250;
            ToryRakietyDo[3] = new int[2]; ToryRakietyDo[3][0] = 150; ToryRakietyDo[3][1] = 400;
            //-----------------------PrawyGornyXGlebokiXLewyGornyYNormalT-------------------
            DrogaX = new Curve();
            DrogaY = new Curve();
            DrogaX.Keys.Add(new CurveKey(0, 300f));
            DrogaX.Keys.Add(new CurveKey(150, 300f));
            DrogaX.Keys.Add(new CurveKey(250, 100f));
            DrogaX.Keys.Add(new CurveKey(400, 100f));

            DrogaY.Keys.Add(new CurveKey(0, -100f));
            DrogaY.Keys.Add(new CurveKey(150, 300f));
            DrogaY.Keys.Add(new CurveKey(250, 300f));
            DrogaY.Keys.Add(new CurveKey(400, -100f));
            DrogaX.ComputeTangents(CurveTangent.Smooth);
            DrogaY.ComputeTangents(CurveTangent.Smooth);
            ToryX[4] = DrogaX;
            ToryY[4] = DrogaY;
            ToryT[4] = 400;
            ToryLaseryOd[4] = new int[1]; ToryLaseryOd[4][0] = 150;
            ToryLaseryDo[4] = new int[1]; ToryLaseryDo[4][0] = 250;
            ToryRakietyOd[4] = new int[2]; ToryRakietyOd[4][0] = 0; ToryRakietyOd[4][1] = 250;
            ToryRakietyDo[4] = new int[2]; ToryRakietyDo[4][0] = 150; ToryRakietyDo[4][1] = 400;
            //-----------------------PoziomYNormalT-------------------
            DrogaX = new Curve();
            DrogaY = new Curve();
            DrogaX.Keys.Add(new CurveKey(0, -50f));
            DrogaX.Keys.Add(new CurveKey(400, 600f));

            DrogaY.Keys.Add(new CurveKey(0, 400f));
            DrogaY.Keys.Add(new CurveKey(400, 400f));
            DrogaX.ComputeTangents(CurveTangent.Smooth);
            DrogaY.ComputeTangents(CurveTangent.Smooth);
            ToryX[5] = DrogaX;
            ToryY[5] = DrogaY;
            ToryT[5] = 400;
            ToryLaseryOd[5] = new int[1]; ToryLaseryOd[5][0] = 100;
            ToryLaseryDo[5] = new int[1]; ToryLaseryDo[5][0] = 400;
            ToryRakietyOd[5] = new int[1]; ToryRakietyOd[5][0] = 0; 
            ToryRakietyDo[5] = new int[1]; ToryRakietyDo[5][0] = 200;
            //-----------------------SciezkaBossa-------------------
            DrogaX = new Curve();
            DrogaY = new Curve();
            DrogaX.Keys.Add(new CurveKey(0, 0f));
            DrogaX.Keys.Add(new CurveKey(10, 0f));
            DrogaX.Keys.Add(new CurveKey(300, 240f));

            DrogaY.Keys.Add(new CurveKey(0, -150f));
            DrogaY.Keys.Add(new CurveKey(10, 0f));
            DrogaY.Keys.Add(new CurveKey(300, 300f));
            DrogaX.ComputeTangents(CurveTangent.Smooth);
            DrogaY.ComputeTangents(CurveTangent.Smooth);
            ToryX[6] = DrogaX;
            ToryY[6] = DrogaY;
            ToryT[6] = 1800;
            ToryLaseryOd[6] = new int[1]; ToryLaseryOd[6][0] = 300;
            ToryLaseryDo[6] = new int[1]; ToryLaseryDo[6][0] = 1800;
            ToryRakietyOd[6] = new int[1]; ToryRakietyOd[6][0] = 300;
            ToryRakietyDo[6] = new int[1]; ToryRakietyDo[6][0] = 1800;
        }

        protected static void InitFal()
        {
 
            //-----------------------RakietowaNamierzanaSlaba-------------------
            List<StatekInformacja> statki = new List<StatekInformacja>();
            statki.Add(new StatekInformacja(Statki[0].Kopia(), 0, null, null, null, null, 90, 1, 1.0f, false, true));
            statki.Add(new StatekInformacja(Statki[0].Kopia(), -20, null, null, null, null, 60, 1, 1.0f, false, true));
            Fale[0] = new FalaInformacyjna(statki);
            //-----------------------RakietowaNamierzanaSlaba-------------------
            statki = new List<StatekInformacja>();
            statki.Add(new StatekInformacja(Statki[0].Kopia(), 0, null, null, null, null, 0, 30, 1.0f, true, false));
            statki.Add(new StatekInformacja(Statki[0].Kopia(), -30, null, null, null, null, 0, 30, 1.0f, true, false));
            Fale[1] = new FalaInformacyjna(statki);
            //-----------------------LaserNamierzalnaRakietowaNamierzalnaSlaba-------------------
            statki = new List<StatekInformacja>();
            statki.Add(new StatekInformacja(Statki[0].Kopia(), 0, null, null, null, null, 90, 30, 1.0f, true, true));
            statki.Add(new StatekInformacja(Statki[0].Kopia(), -30, null, null, null, null, 60, 30, 1.0f, true, true));
            Fale[2] = new FalaInformacyjna(statki);
            //-----------------------LaserNamierzalnaRakietowaNamierzalnaSlabaSzybka-------------------Duzo przeciwnikow ps
            statki = new List<StatekInformacja>();
            statki.Add(new StatekInformacja(Statki[0].Kopia(), 0, null, null, null, null, 90, 60, 2.0f, true, true));
            statki.Add(new StatekInformacja(Statki[0].Kopia(), -30, null, null, null, null, 90, 60, 2.0f, true, true));
            statki.Add(new StatekInformacja(Statki[0].Kopia(), -60, null, null, null, null, 90, 30, 2.0f, true, true));
            statki.Add(new StatekInformacja(Statki[0].Kopia(), -90, null, null, null, null, 60, 30, 2.0f, true, true));
            Fale[4] = new FalaInformacyjna(statki);
            ////-----------------------LaserNamierzalnyRakietyNienamierzalneSredni-------------------
            statki = new List<StatekInformacja>();
            statki.Add(new StatekInformacja(Statki[1].Kopia(), 0, null, null, null, null, 90, 30, 0.5f, true, true));
            statki.Add(new StatekInformacja(Statki[1].Kopia(), -60, null, null, null, null, 90, 30, 0.5f, true, true));
            statki.Add(new StatekInformacja(Statki[1].Kopia(), -120, null, null, null, null, 90, 30, 0.5f, true, true));
            statki.Add(new StatekInformacja(Statki[1].Kopia(), -200, null, null, null, null, 90, 30, 0.5f, true, true));
            Fale[3] = new FalaInformacyjna(statki);
            ////-----------------------BOSS-------------------
            statki = new List<StatekInformacja>();
            statki.Add(new StatekInformacja(Statki[2].Kopia(), 0, null, null, null, null, 1, 1, 0.5f, true, true));
            Fale[5] = new FalaInformacyjna(statki);
        }


        protected static void InitStatki()
        {
            //-----------------------LHLS-------------------
            PowerUp[] Bonusy = new PowerUp[4];
            Color start = Color.Green;
            Color end = start;
            end.A = 50;
            Bonusy[0] = Powers.Double;
            Bonusy[1] = Powers.Laser;
            Bonusy[2] = Powers.Brown;
            Bonusy[3] = Powers.Blue;
            Statki[0] = new StatekWroga(new Vector2(200f,200f), ref Grafika.StatekGracza, 0f,
                                            50f, 0.2f, 20f,1, 3, true, true, 200,Bonusy,500,
                                            new ParticleGun(0, 0, 2, 4, 2, 4, start, Color.GreenYellow, end, end,
                -MathHelper.PiOver4, MathHelper.PiOver4,
                60f, 120f, 10, 20, 0, Rodzaj_Ruchu.RuchJednostajny, 1, Particle.Storage.Rodzaj_Dziala.Losowo));
            ////-----------------------MHMS-------------------
            Bonusy = new PowerUp[4];
            start = Color.White;
            end = start;
            end.A = 50;
            Bonusy[0] = Powers.Double;
            Bonusy[1] = Powers.Laser;
            Bonusy[2] = Powers.Red;
            Bonusy[3] = Powers.Red;
            Statki[1] = new StatekWroga(new Vector2(200f, 200f), ref Grafika.Statki[0], 0f,
                                            150f, 0.5f, 50f, 0, 2, true,false, 200, Bonusy, 1000,
                                            new ParticleGun(0, 0, 2, 4, 2, 4, start, Color.White, end, end,
                -MathHelper.PiOver4, MathHelper.PiOver4,
                60f, 120f, 10, 20, 0, Rodzaj_Ruchu.RuchJednostajny, 1, Particle.Storage.Rodzaj_Dziala.Losowo));
            //-----------------------BOSS-------------------
            Statki[2] = new BossCzacha();


        }

        protected static void MapInit()
        {

            //----------------------MAPA0-----------------
            List<FalaInformacyjna> Fale = new List<FalaInformacyjna>();
            List<int> Kiedy = new List<int>();
            Fale.Add(StworzFale((byte)RodzajeFal.BOSS, (byte)RodzajeTorow.SciezkaBossa));
            Kiedy.Add(0);
            ListaMap[0] = new Mapa(9000, true, Fale, Kiedy);
            //----------------------MAPA1-----------------
            Fale = new List<FalaInformacyjna>();
            new List<int>();
            Fale.Add(StworzFale((byte)RodzajeFal.LaserNamierzalnaRakietowaNamierzalnaSlaba, (byte)RodzajeTorow.PoziomYNormalT));
            Kiedy.Add(0);
            ListaMap[1] = new Mapa(9000, true, Fale, Kiedy);
            //----------------------MAPA2-----------------
            Fale = new List<FalaInformacyjna>();
            new List<int>();
            Fale.Add(StworzFale((byte)RodzajeFal.LaserNamierzalnaRakietowaNamierzalnaSlaba, (byte)RodzajeTorow.LewaGornaXSrodekXGornaPrawaYNormalT));
            Kiedy.Add(0);
            ListaMap[2] = new Mapa(9000, true, Fale, Kiedy);

            

           
        }

        protected static FalaInformacyjna StworzFale(byte FI,byte Tor)
        {
            return Fale[FI].StworzFale(ToryX[Tor], ToryY[Tor], ToryT[Tor], ToryLaseryOd[Tor], ToryLaseryDo[Tor], ToryRakietyOd[Tor], ToryRakietyDo[Tor]);
        }


    }
}