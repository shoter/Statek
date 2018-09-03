using Damian.MWGK.Obiekt.Powers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Damian.MWGK.Global.Zbiory
{
    class Powers
    {
        public static PowerUpBlue Blue = new PowerUpBlue(Vector2.Zero,0f);
        public static PowerUpBrown Brown = new PowerUpBrown(Vector2.Zero, 0f);
        public static PowerUpDouble Double = new PowerUpDouble(Vector2.Zero, 0f);
        public static PowerUpGreen Green = new PowerUpGreen(Vector2.Zero, 0f);
        public static PowerUpLaser Laser = new PowerUpLaser(Vector2.Zero, 0f);
        public static PowerUpRed Red = new PowerUpRed(Vector2.Zero, 0f);
    }
}
