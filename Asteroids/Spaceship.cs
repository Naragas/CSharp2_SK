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
        private int _energy = 100;
        public int Energy => _energy;
        public void EnergySetDefaul()
        {
            _energy = 100;
        }
        public void EnergyLow(int n)
        {
            _energy -= n;
        }
        public void EnergyRecover(int n)
        {
            _energy += n;
        }
        static Image spaceShip = Image.FromFile("spaceship.png");

        public Spaceship(Point pos,Point dir, Size size) : base(pos, dir, size)
        {
            
        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(spaceShip,Pos.X,Pos.Y);
        }

        public override void NiceShot()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            return;
        }
        public void MoveHorizontal( int Direction)
        {
            Pos.X += Direction*Dir.X;
        }

        public void MoveVertical(int Direction)
        {
            Pos.Y += Direction*Dir.Y;
        }
        public void Die()
        {
            
        }
    }
}
