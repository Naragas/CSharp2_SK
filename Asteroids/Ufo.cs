﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Asteroids
{
    class Ufo : BaseObject
    {
        static Image ufo = Image.FromFile("ufo.png");

        int a;
        public Ufo(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            a = Pos.Y;
        }


        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(ufo, Pos.X, Pos.Y);
        }

        public void NewPossition()
        {
            a = Game.rnd.Next(50, 550);
        }
        public override void NiceShot()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {

            Pos.X += (Dir.X - 2);
            Pos.Y = (int)Math.Round(a + 50 * Math.Sin(25 * Pos.X));
            if (Pos.X < (0 - 50))
            {
                Pos.X = Game.Width + Game.rnd.Next(100,400);
                NewPossition(); //Как сделать так чтобы каждый экземпляр появляся со своей координатой?
            }
        }
    }
}
