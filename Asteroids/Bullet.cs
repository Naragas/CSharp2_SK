using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Asteroids
{
    class Bullet : BaseObject
    {        
        static Image bullet = Image.FromFile("bullet.png");
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {

        }

        public override void NiceShot()
        {
            Pos.X = 0;
            Pos.Y = Game.rnd.Next(0, Game.Height);
        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(bullet, Pos.X, Pos.Y);
        }

        public override void Update()
        {
            Pos.X +=Dir.X;

        }
    }
}
