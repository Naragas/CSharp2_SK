using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Asteroids
{
    public delegate void Logger(String message);
    static class Game
    {
        public static Random rnd = new Random();
        private static readonly Image background = Image.FromFile("background.png");
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        private static Timer timer;


        private static Logger logger = new Logger(LoggingToConsole);
        private static DateTime dt;
        private static int Score = 0; // переменная хранящая в себе итоговый результат награды за сбитые астероиды.
        private static int Number; //Переменна отвечающая за стартовое количество астероидов в волне.
        private static readonly string GameStatus = "WASD - движение корабля. Space - стрельба, P - Пауза."; //подсказка по управлению

        //Элементы управления
        private static bool MoveLeft = false;
        private static bool MoveRight = false;
        private static bool MoveUp = false;
        private static bool MoveDown = false;
        private static bool Fire = false;
        private static bool GameOnPause = false;

        // Объекты
        private static Spaceship Spaceship;
        private static HealPack _healpack;
        private static List<Bullet> _bullets = new List<Bullet>();
        private static List<Asteroid> _asteroids;




        
        public static void Load()
        {
            Number = 4; 
            logger += new Logger(LoggingToFile);
            _asteroids = new List<Asteroid>();
            Spaceship = new Spaceship(new Point(0, 300), new Point(5, 5), new Size(40, 30));
            _healpack = new HealPack(new Point(1500, rnd.Next(0, Game.Height)), new Point(-5, 0), new Size(40, 40));

            GetAsteroidVawe(ref Number);
            #region
            //for (int i = 1; i < 5; i++)
            //{
            //    _objs[i] = new Ufo(new Point(800 + rnd.Next(50, 500), rnd.Next(50, (Height - 50))), new Point(-2, 0), new Size(5, 5));

            //}
            //for (int i = 5; i < _objs.Length; i++)
            //{
            //    int r = rnd.Next(5, 50);
            //    _objs[i] = new Star(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r, r), new Size(3, 3));
            //}
            #endregion
        }

        /// <summary>
        /// Метод добавляющий новую волну астероидов в коллекцию, и увеличивающий параметр отвечающий за размер коллекции на единицу.
        /// </summary>
        /// <param name="Count">Параметр отвечающий за размер коллекции.</param>
        private static void GetAsteroidVawe(ref int Count)
        {
            for (int i = 0; i < Count; i++)
            {
                int r = rnd.Next(5, 50);
                _asteroids.Add(new Asteroid(new Point(rnd.Next(1000, 1600), rnd.Next(0, Game.Height)), new Point(-5, 0), new Size(r, r)));
            }
            Count++;
        }

        public static int Width { get; set; }
        public static int Height { get; set; }
        /// <summary>
        /// Метод ведущий логирование в консоль.
        /// </summary>
        /// <param name="message">Сообщение о совершенном действии</param>
        private static void LoggingToConsole(String message)
        {
            Console.WriteLine((dt = DateTime.Now) + " " + message);
        }
        /// <summary>
        /// Метод ведущий логирование в файл.
        /// </summary>
        /// <param name="message">Сообщение о совершенном действии</param>
        private static void LoggingToFile(String message)
        {
            StreamWriter sw = new StreamWriter("log.txt", true);
            sw.WriteLine((dt = DateTime.Now) + " " + message);
            sw.Close();
        }

        /// <summary>
        /// GameWindowSizeCheck - проверяет корректность размеров игровой формы.
        /// </summary>
        /// <param name="width">Ширина формы, которую пытаются задать</param>
        /// <param name="height">Высота  формы, которую пытаются задать</param>
        private static void GameWindowSizeCheck(int width, int height)
        {
            if (
                width < 0
                || width > 1000
                || height < 0
                || height > 1000
                )
            {
                throw new ArgumentOutOfRangeException("Размер экрана должен быть в пределах от 0 до 1000");
            }
        }

        static Game() { }
        /// <summary>
        /// Метод инициализации игрового поля.
        /// </summary>
        /// <param name="form">Форма Windows Forms</param>
        public static void Init(Form form)
        {

            Graphics g;
            timer = new Timer { Interval = 100 };
            timer.Start();
            timer.Tick += Timer_Tick;
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;
            GameWindowSizeCheck(Width, Height);
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            KeyListener(form);
            Load();
            logger($"Игра началась!");


        }
        /// <summary>
        /// Метод отвечающий за прорисовку всех объектов во время игры.
        /// </summary>
        public static void Draw()
        {
            Buffer.Graphics.DrawImage(background, new Point(0, 0));
            Buffer.Graphics.DrawString($"Score: {Score}   Asteroids in wave: {_asteroids.Count}", SystemFonts.DefaultFont, Brushes.White, 70, 0);
            Buffer.Graphics.DrawString(GameStatus, SystemFonts.DefaultFont, Brushes.White, 400, 0);
            for (int i = 0; i < _asteroids.Count; i++)
            {
                _asteroids[i].Draw();
            }

            foreach (Bullet bullet in _bullets) bullet.Draw();
            _healpack.Draw();
            Spaceship.Draw();
            if (Spaceship != null)
            {
                Buffer.Graphics.DrawString("Energy:" + Spaceship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
            }

            Buffer.Render();

        }
        /// <summary>
        /// Метод обновляющий позициии всех объектов во время игры.
        /// </summary>
        public static void Update()
        {
            _healpack?.Update();
            for (int i = 0; i < _bullets.Count; i++)
            {
                //Проверка на вылет пули за границы игрового поля.
                _bullets[i]?.Update();
                if (_bullets[i].Rect.X > Width)
                {
                    _bullets.RemoveAt(i);
                }
            }

            Spaceship.Update();

            for (int i = 0; i < _asteroids.Count; i++)
            {
                _asteroids[i]?.Update();
                for (int j = 0; j < _bullets.Count; j++)
                {
                    if (_asteroids.Count == 0) break;

                    //Проверка на попадание по астероиду, в зависимости от размера астеройдов нужно несколько попаданий.
                    if (_bullets[j].Collision(_asteroids[i]))
                    {
                        _bullets.RemoveAt(j);
                        System.Media.SystemSounds.Hand.Play();
                        logger("Попадание по астеройду.");                       

                        _asteroids[i].Power--;

                        if (_asteroids[i].Power <= 0)
                        {
                            Score += _asteroids[i].Reward;
                            logger($"Астеройд уничтожен. начислено {_asteroids[i].Reward}");
                            _asteroids.RemoveAt(i);
                            if (i != 0) i--;
                            if (j != 0) j--;
                            continue;
                        }

                    }

                }
                if (_asteroids.Count == 0) break;

                // Проверка на столкновение корабля и астеройдов. снижение энергии в зависимости от размера астероида.
                if (_asteroids.Count != 0 && !Spaceship.Collision(_asteroids[i])) continue;
                Spaceship?.EnergyLow(_asteroids[i].Power * 5);
                logger($"Столконовение с астеройдом, энергия корабля {Spaceship.Energy}");
                System.Media.SystemSounds.Asterisk.Play();
                _asteroids.RemoveAt(i);
                logger("Астеройд уничтожен после столкновения.");
                if (Spaceship.Energy <= 0)
                {
                    Spaceship?.Die();
                    timer.Stop();
                    logger("Корабль уничтожен. Игра окончена.");
                    WriteGameMessage("Корабль уничтожен. Вы проиграли!");
                }
            }
            // Проверка на столкновение  с аптечкой.
            if (Spaceship.Collision(_healpack))
            {
                Spaceship.EnergyRecover(_healpack.HealPower);
                if (Spaceship.Energy > 100) Spaceship.EnergySetDefaul();
                logger($"Подобрана аптечка, энергия корабля {Spaceship.Energy}");
                _healpack.NiceShot();
            }
            //Проверка на окончание волны астероидов.
            if (_asteroids.Count == 0)
            {
                AllAsteroidsIsDestoroyed();
            }
        }
        /// <summary>
        /// Метод информирующий об окончании одной волны астеройдов и создающий новую волну.
        /// </summary>
        private static async void AllAsteroidsIsDestoroyed()
        {
            WriteGameMessage
                (
                "Волна астеройдов уничтожена.\n" +
                "Появляется следующая волна."
                );
            timer.Stop();
            await Task.Delay(5000);
            timer.Start();
            GetAsteroidVawe(ref Number);
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            GameControls();
            Draw();
            Update();
        }


        /// <summary>
        /// Метод отлавливающий нажатия клавишь.
        /// </summary>
        /// <param name="form"></param>
        private static void KeyListener(Form form)
        {
            form.KeyDown += (object sender, KeyEventArgs e) =>
            {
                if (e.KeyCode == Keys.Space) Fire = true;

                if (e.KeyCode == Keys.W) MoveUp = true;

                if (e.KeyCode == Keys.S) MoveDown = true;

                if (e.KeyCode == Keys.A) MoveLeft = true;

                if (e.KeyCode == Keys.D) MoveRight = true;

                if (e.KeyCode == Keys.P)
                {
                    if (!GameOnPause)
                    {
                        GameOnPause = true;
                        timer.Stop();
                        logger("Игра приостановлена");
                        WriteGameMessage("Игра на паузе, нажмите P для продолжения");
                    }
                    else
                    {
                        GameOnPause = false;
                        timer.Start();
                        logger("Игра возобновлена.");
                    }
                }
            };
            form.KeyUp += (object sender, KeyEventArgs e) =>
            {
                if (e.KeyCode == Keys.Space) Fire = false;

                if (e.KeyCode == Keys.W) MoveUp = false;

                if (e.KeyCode == Keys.S) MoveDown = false;

                if (e.KeyCode == Keys.A) MoveLeft = false;

                if (e.KeyCode == Keys.D) MoveRight = false;

            };
        }
        /// <summary>
        /// Метод управления кораблем.
        /// </summary>
        private static void GameControls()
        {
            if (Fire)
            {
                if (_bullets.Count < 10)
                {
                    _bullets.Add(new Bullet(new Point(Spaceship.Rect.X + 30, Spaceship.Rect.Y + 15), new Point(7, 0), new Size(20, 10)));
                    logger("Произведен выстрел");
                }
                else
                {
                    logger("Не хватает патронов для выстрела.");
                }
            }
            if (MoveUp) Spaceship.MoveVertical(-1);

            if (MoveDown) Spaceship.MoveVertical(1);

            if (MoveLeft) Spaceship.MoveHorizontal(-1);

            if (MoveRight) Spaceship.MoveHorizontal(1);

        }

        /// <summary>
        /// Метод выводит сообщение для пользователя в центре экрана.
        /// </summary>
        /// <param name="message">сообщение для пользователя</param>
        private static void WriteGameMessage(string message)
        {
            Font Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Underline);
            SizeF MessageSize = Buffer.Graphics.MeasureString(message, Font);
            Buffer.Graphics.DrawString(message, Font, Brushes.White, (Width - MessageSize.Width) / 2, Height / 2);
            Buffer.Render();
        }

    }
}
