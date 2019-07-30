using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Asteroids
{
    class HealPack : BaseObject
    {
        static Image healpack = Image.FromFile("healpack.png");
        
        //HealPower - параметр отвечающий за то сколько энергии будет восстановлено кораблю.
        public int HealPower { get; }
        public HealPack(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            HealPower = 30;
        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(healpack, Pos.X, Pos.Y);
        }


        /// <summary>
        /// Метод восстанавливает аптечку после того как она была поймана кораблем.
        /// </summary>
        public override void NiceShot()
        {
            Pos.X = 2000;
            Pos.Y = Game.rnd.Next(40, Game.Height - 40);
        }

        public override void Update()
        {
            Pos.X += Dir.X;

            if (Pos.X < (-40)) Pos.X = Game.Width + 700;

        }
    }
}
