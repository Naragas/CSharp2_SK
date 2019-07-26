using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Asteroids
{
    
    class Asteroid : BaseObject, ICloneable, IComparable<Asteroid>
    {
        static Image asteroidLow = Image.FromFile("asteroidLow.png");
        static Image asteroidMedium = Image.FromFile("asteroidMedium.png");
        static Image asteroidLarge = Image.FromFile("asteroidlarge.png");
        public int Power { get; set; }
        public int Reward { get; set; }
        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            GetPowerFromSize();
        }
        public override void Draw()
        {
            SizeAndPowerRelation();

        }

        private void SizeAndPowerRelation()
        {
            if (Power == 1)
            {
                Game.Buffer.Graphics.DrawImage(asteroidLow, Pos.X, Pos.Y);
            }
            if (Power == 2)
            {
                Game.Buffer.Graphics.DrawImage(asteroidMedium, Pos.X, Pos.Y);
            }
            if (Power == 3)
            {
                Game.Buffer.Graphics.DrawImage(asteroidLarge, Pos.X, Pos.Y);
            }
        }

        private void GetPowerFromSize()
        {
            if (Size.Width >= 0 && Size.Width < 25)
            {
                Power = 1;
                Reward = Power * 5;
                Game.Buffer.Graphics.DrawImage(asteroidLow, Pos.X, Pos.Y);
            }
            if (Size.Width >= 25 && Size.Width < 40)
            {
                Power = 2;
                Reward = Power * 5;
                Game.Buffer.Graphics.DrawImage(asteroidMedium, Pos.X, Pos.Y);
            }
            if (Size.Width >= 40 && Size.Width <= 50)
            {
                Power = 3;
                Reward = Power * 5;
                Game.Buffer.Graphics.DrawImage(asteroidLarge, Pos.X, Pos.Y);
            }
        }

        public override void Update()
        {
            Pos.X += Dir.X;
            Pos.Y += Dir.Y;
            if (Pos.X < (0 - 40)) Pos.X = Game.Width + 200;
            if (Pos.Y > Game.Height || Pos.Y < 0) Dir.Y = -Dir.Y;
        }

        public override void NiceShot()
        {
            Pos.X = Game.Width + 100;
        }

        public object Clone()
        {
            Asteroid asteroid = new Asteroid(new Point(Pos.X, Pos.Y), new Point(Dir.X, Dir.Y), new Size(Size.Width, Size.Height)) { Power = Power };
            return asteroid;
        }

        int IComparable<Asteroid>.CompareTo(Asteroid obj)
        {
            if (Power > obj.Power)
                return 1;
            if (Power < obj.Power)
                return -1;
            return 0;
        }

    }
}
