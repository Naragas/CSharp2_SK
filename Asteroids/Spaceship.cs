using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Asteroids
{
    class Spaceship : BaseObject
    {
        static Image spaceShip = Image.FromFile("spaceship.png");

        public Spaceship(Point pos,Point dir, Size size) : base(pos, dir, size)
        {

        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(spaceShip,Pos.X,Pos.Y);
        }

        public override void Update()
        {
            
        }

    }
}
