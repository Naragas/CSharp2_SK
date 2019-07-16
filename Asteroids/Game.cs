using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Asteroids
{
    static class Game
    {
        static Random rnd = new Random();
        static Image background = Image.FromFile("background.png");
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        public static BaseObject[] _objs;
        public static void Load()
        {
            _objs = new BaseObject[30];
            for (int i = 0; i < _objs.Length; i++)
            {
                _objs[i] = new Star(new Point(rnd.Next(200, Width), rnd.Next(0, 600)), new Point(i, 0), new Size(5, 5));
            }
        }
        public static int Width { get; set; }
        public static int Height { get; set; }

        static Game() { }

        public static void Init(Form form)
        {
            
            Graphics g;
            Timer timer = new Timer { Interval = 100 };
            timer.Start();
            timer.Tick += Timer_Tick;

            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();

            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;

            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            Load();

        }

        public static void Draw()
        {
            Buffer.Graphics.DrawImage(background, new Point(0, 0));
            //Buffer.Graphics.DrawRectangle(Pens.White, new Rectangle(100, 100, 200, 200));
            //Buffer.Graphics.FillEllipse(Brushes.Wheat, new Rectangle(100, 100, 200, 200));
            foreach (BaseObject obj in _objs)
            {
                obj.Draw();
            }
            Buffer.Render();
        }

        public static void Update()
        {
            foreach (BaseObject obj in _objs)
            {
                obj.Update();
            }
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
    }
}
