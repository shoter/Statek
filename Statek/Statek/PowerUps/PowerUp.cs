using Damian.MWGK.Obiekt;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Damian.MWGK.Global.Test;
using Damian.MWGK.Global.Graphics;
namespace Damian.MWGK.Obiekt.Powers
{
    class PowerUp : Obiekt
    {
        public PowerUp(Vector2 pozycja, ref Texture2D obrazek, float obrot)
            : base(pozycja,ref obrazek, obrot)
        { }

        public override void Pracuj()
        {
            base.Pracuj();
            if (Zniszczony) return;
            if (Kolizja(GlobalAcc.StatekGracza))
            {
                PowerUpZdobyty();
            }
            if (Pozycja.Y > 1000f || Pozycja.Y < -200f)
                Zniszczony = true;
        }

        protected virtual void PowerUpZdobyty()
        { }

        public virtual PowerUp Kopia(Vector2 pozycja,float obrot)
        {
            return new PowerUp(pozycja,ref Obrazek, obrot);
        }

        

    }
    /// <summary>
    /// Podwaja ilosc zdobywanych punktow
    /// </summary>
    class PowerUpDouble : PowerUp
    {
        public PowerUpDouble(Vector2 pozycja, float obrot)
        :base(pozycja,ref Grafika.PowerUps[1],obrot)
        {  }

        protected override void PowerUpZdobyty()
        {
            GlobalAcc.StatekGracza.DoublePoints.Aktywuj();
            Zniszczony = true;
        }

        public override PowerUp Kopia(Vector2 pozycja, float obrot)
        {
            return new PowerUpDouble(pozycja,obrot);
        }

    }

    class PowerUpLaser : PowerUp
    {
        public PowerUpLaser(Vector2 pozycja, float obrot)
            : base(pozycja, ref Grafika.PowerUps[2], obrot)
        { }

        protected override void PowerUpZdobyty()
        {
            if (GlobalAcc.StatekGracza.AktLaser != 2)
            {
                GlobalAcc.StatekGracza.AktLaser++;
                GlobalAcc.StatekGracza.IDLasera = GlobalAcc.StatekGracza.AktLaser;
            }
            else
            {
                GlobalAcc.StatekGracza.IloscPkt += 1000;
                if(GlobalAcc.StatekGracza.DoublePoints.active)
                    GlobalAcc.StatekGracza.IloscPkt += 1000;
            }
            Zniszczony = true;

        }

        public override PowerUp Kopia(Vector2 pozycja, float obrot)
        {
            return new PowerUpLaser(pozycja, obrot);
        }
    }

    class PowerUpBlue : PowerUp
    {
        public PowerUpBlue(Vector2 pozycja, float obrot)
            : base(pozycja, ref Grafika.PowerUps[3], obrot)
        { }

        protected override void PowerUpZdobyty()
        {
                GlobalAcc.StatekGracza.IloscPkt += 500;
                if(GlobalAcc.StatekGracza.DoublePoints.active)
                    GlobalAcc.StatekGracza.IloscPkt += 500;
            Zniszczony = true;

        }

        public override PowerUp Kopia(Vector2 pozycja, float obrot)
        {
            return new PowerUpBlue(pozycja, obrot);
        }
    }

    class PowerUpBrown : PowerUp
    {
        public PowerUpBrown(Vector2 pozycja, float obrot)
            : base(pozycja, ref Grafika.PowerUps[4], obrot)
        { }

        protected override void PowerUpZdobyty()
        {
            GlobalAcc.StatekGracza.IloscPkt += 100;
            if (GlobalAcc.StatekGracza.DoublePoints.active)
                GlobalAcc.StatekGracza.IloscPkt += 100;
            Zniszczony = true;

        }

        public override PowerUp Kopia(Vector2 pozycja, float obrot)
        {
            return new PowerUpBrown(pozycja, obrot);
        }
    }

    class PowerUpGreen : PowerUp
    {
        public PowerUpGreen(Vector2 pozycja, float obrot)
            : base(pozycja, ref Grafika.PowerUps[5], obrot)
        { }

        protected override void PowerUpZdobyty()
        {
            GlobalAcc.StatekGracza.IloscPkt += 1000;
            if (GlobalAcc.StatekGracza.DoublePoints.active)
                GlobalAcc.StatekGracza.IloscPkt += 1000;
            Zniszczony = true;

        }

        public override PowerUp Kopia(Vector2 pozycja, float obrot)
        {
            return new PowerUpGreen(pozycja, obrot);
        }
    }

    class PowerUpRed : PowerUp
    {
        public PowerUpRed(Vector2 pozycja, float obrot)
            : base(pozycja, ref Grafika.PowerUps[6], obrot)
        { }

        protected override void PowerUpZdobyty()
        {
            GlobalAcc.StatekGracza.IloscPkt += 2000;
            if (GlobalAcc.StatekGracza.DoublePoints.active)
                GlobalAcc.StatekGracza.IloscPkt += 2000;
            Zniszczony = true;

        }

        public override PowerUp Kopia(Vector2 pozycja, float obrot)
        {
            return new PowerUpRed(pozycja, obrot);
        }
    }

}