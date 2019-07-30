using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Asteroids
{
    abstract class BaseObject : ICollision
    {
        
        protected Point Pos;
        protected Point Dir;
        protected Size Size;

         

        protected BaseObject(Point pos,Point dir,Size size)
        {
            if(
                pos.X <0
                || pos.Y <0
                || size.Height <0
                || size.Width < 0
               )
            {
                throw new GameObjectException("Заданы неверные параметры объекта");
            }
            Pos = pos;
            Dir = dir;
            Size = size;
            
        }        

        public bool Collision(ICollision o) => o.Rect.IntersectsWith(this.Rect);
        public Rectangle Rect => new Rectangle(Pos, Size);

        /// <summary>
        /// Метод прорисовки объекта.
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Метод обновления координат объекта
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Меток NiceShot переопределяет положение объекта после попадания пули или столконовения с кораблем.
        /// </summary>
        public abstract void NiceShot();

    }
}
