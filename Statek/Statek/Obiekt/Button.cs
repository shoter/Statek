using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Damian.MWGK.Obiekt;
using Damian.MWGK.funkcje;

namespace Damian.MWGK.Obiekt
{

    class Button : Obiekt
    {
        Texture2D ObrazekWcisniety; //obrazek wykorzystywany jako nie wcisniety obraz

        public bool Wcisniety { private set; get; }
        private bool Pobrany; //za sekunde bedzie potrzebny
        /// <summary>
        /// Zwraca czy guzik byl kliniety,nastepny zwrot zawsze zwroci false,nie wazne czy guzik jest wcisniety czy nie
        /// </summary>
        public bool Pojedyncze_wcisniecie
        {
            get
            {
                if (Wcisniety)
                {
                    if (Pobrany == true)
                        return false;
                    else
                    {
                        Pobrany = true;
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Tworzy nowy button
        /// </summary>
        /// <param name="pozycja">Pozycja buttona lewy gorny rog</param>
        /// <param name="obrazek">Obrazek nie wcisnietego buttona</param>
        /// <param name="obrazekwcisniety">Obrazek wcisnietego buttona</param>
        /// <param name="CM">content manager</param>
        /// <param name="obrot">obrot przycisku</param>
        public Button(Vector2 pozycja, string obrazek, string obrazekwcisniety, ContentManager CM, float obrot)
            : base(pozycja, obrazek, CM, obrot)
        {
            ObrazekWcisniety = CM.Load<Texture2D>(obrazekwcisniety);
        }
        public Button(Vector2 pozycja, ref Texture2D obrazek, ref Texture2D obrazekwcisniety, float obrot)
            : base(pozycja,ref obrazek, obrot)
        {
            ObrazekWcisniety = obrazekwcisniety;
        }

        /// <summary>
        /// Sprwadza czy button zostal dotkniety przez palce
        /// </summary>
        /// <param name="gdzie">zawiera tablice miejsc gdzie palec dotyka ekranu</param>
        public void Pracuj(Vector2[] gdzie,int ile)
        {
            bool dotkniety = false;
            for(int i = 0;i < ile;i++)
            {
                Vector2 X = gdzie[i];
                if (Funkcje.kolizja(X, 1, 1, Pozycja - rozmiary / 2, (int)rozmiary.X, (int)rozmiary.Y))
                {
                    dotkniety = true;
                    break;
                }

            }

            if (dotkniety)
            {
                Wcisniety = true;
            }
            else if (Wcisniety)
                {
                    Wcisniety = false;
                    Pobrany = false;
                }

        }

        public virtual void rysuj(ref SpriteBatch SB)
        {

            if(Wcisniety)
                SB.Draw(ObrazekWcisniety, Pozycja, null, Color.White, Obrot, Srodek, Vector2.One, SpriteEffects.None, 0);     
            else
            SB.Draw(Obrazek, Pozycja, null, Color.White, Obrot, Srodek, Vector2.One, SpriteEffects.None, 0);     
        }

    }
}