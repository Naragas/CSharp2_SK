﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Asteroids
{
    class BaseObject
    {
        
        protected Point Pos;
        protected Point Dir;
        protected Size Size;

         

        public BaseObject(Point pos,Point dir,Size size)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
            
        }

        public virtual void Draw()
        {
            Game.Buffer.Graphics.DrawEllipse(Pens.White, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public virtual void MoveTop()
        {
            Dir.Y -= 1;
        }
        public virtual void MoveBot()
        {
            Dir.Y += 1;
        }
        public virtual void Update()
        {
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;
            if (Pos.X < 0) Dir.X = -Dir.X;
            if (Pos.X > Game.Width) Dir.X = -Dir.X;
            if (Pos.Y < 0) Dir.Y = -Dir.Y;
            if (Pos.Y > Game.Height) Dir.Y = -Dir.Y;
        }
    }
}
